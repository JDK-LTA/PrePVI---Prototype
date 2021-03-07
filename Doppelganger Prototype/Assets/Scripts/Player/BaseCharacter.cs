using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BinaryInputs { DASH, JUMP, ATTACK1_STATIC, ATTACK1_MOVE, ATTACK2 }
[RequireComponent(typeof(CharacterController))]
public class BaseCharacter : MonoBehaviour
{
    [Header("Basic Movement")]
    [SerializeField] protected float xMoveSpeed = 16f / 3f, zMoveSpeed = 16f / 3f;
    [SerializeField] protected float jumpHeight = 1.5f;

    [Header("Jump")]
    [SerializeField] protected float groundCheckDistance = 0.12f;
    [SerializeField] protected float groundCheckForJump = 0.4f;
    [SerializeField] protected float gravity = -9.81f;

    [Header("Dash")]
    [SerializeField] protected float dashLenght = 4f;
    [SerializeField] protected float dashTime = 0.2f;
    [SerializeField] protected float dashResetTime = 1f;

    [Header("Knockback")]
    [SerializeField] protected float timeKnockbacking = 0.4f;
    [SerializeField] protected float knockbackDistance = 0.5f;
    [SerializeField] protected float attackKbTime = 0.2f;
    [SerializeField] protected float attackKbDistance = 0.5f;


    [Header("Attack 1")]
    [SerializeField] protected float damage = 5f;
    [SerializeField] protected float timeToReachEnd = 0.5f;
    [SerializeField] protected float cooldown1 = 0.8f;
    [SerializeField] protected Collider attack1Collider;

    [Header("Attack 2")]
    [SerializeField] protected float damage2 = 5f;
    [SerializeField] protected float timeToReachEnd2 = 0.5f;
    [SerializeField] protected float cooldown2 = 0.8f;

    protected CharacterController chCont;
    protected Animator animator;

    protected Vector2 xzInput;
    protected Vector3 velocity;

    protected bool grounded;
    protected bool canJump = true;

    protected Vector3 dashMove, dashOrPos;
    protected float dashT = 0f, dashResetT;
    protected bool canDash = true;
    protected bool dashingNow = false;
    protected bool dashReset = false;

    protected bool knockbacking = false, canKb = true;
    protected float kbT = 0;
    protected Vector3 kbOrPos, kbMove, kbDir;

    protected bool attKbing = false, canAttKb = true;
    protected float attKbT = 0;
    protected Vector3 attKbOrPos, attKbMove, attKbDir;
    
    protected bool recuperating = false;
    protected float recupT = 0;

    protected bool canAttack = true;

    protected bool canDoAnythingElse = true;
    protected float generalT = 0;

    protected static bool rec = false;
    protected static bool canStartRecording = true;

    public Animator Animator { get => animator; set => animator = value; }
    public bool Grounded { get => grounded; set => grounded = value; }
    public float Damage { get => damage; set => damage = value; }
    public float Damage2 { get => damage2; set => damage2 = value; }

    #region BASIC MONOBEHAVIOUR METHODS
    protected virtual void Awake()
    {
        chCont = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        XZMove();
        YMove();
        DashingMove();
        AttackKnockback();
    }
    #endregion
    #region INPUT
    private int aux;
    protected void ContinousInput()
    {
        aux = canDoAnythingElse ? 1 : 0;
        xzInput.x = Input.GetAxis("Horizontal") * aux;
        xzInput.y = Input.GetAxis("Vertical") * aux;
    }
    protected void BinInputs()
    {
        //PRESSES
        if (canAttack)
        {
            if (Input.GetButtonDown("Attack1") && canDoAnythingElse && velocity == Vector3.zero)
            {
                Attack1();
            }
            else if (Input.GetButtonDown("Attack1") && canDoAnythingElse && velocity.magnitude > 0.1f)
            {
                Attack1OnMove();
            }
            if (Input.GetButtonDown("Attack2") && canDoAnythingElse)
            {
                Attack2();
            }
        }
        if (Input.GetButtonDown("Dash") && canDoAnythingElse)
        {
            Dash();
        }
        if (Input.GetButtonDown("Jump") && canDoAnythingElse)
        {
            Jump();
        }
    }
    #endregion
    #region MOVEMENT
    protected virtual void XZMove()
    {
        Vector3 move = new Vector3(xzInput.x * xMoveSpeed, 0, xzInput.y * zMoveSpeed);
        chCont.Move(move * Time.deltaTime);

        if (animator)
            animator.SetFloat("MoveSpeed", Mathf.Clamp01(Mathf.Abs(xzInput.x) + Mathf.Abs(xzInput.y)));

        if (move != Vector3.zero)
        {
            transform.forward = move;
        }
    }
    protected virtual void XZMove(Vector2 inp)
    {
        Vector3 move = new Vector3(inp.x * xMoveSpeed, 0, inp.y * zMoveSpeed);
        chCont.Move(move * Time.deltaTime);

        if (animator)
            animator.SetFloat("MoveSpeed", Mathf.Clamp01(Mathf.Abs(inp.x) + Mathf.Abs(inp.y)));

        if (move != Vector3.zero)
        {
            transform.forward = move;
        }
    }
    protected void YMove()
    {
        if (velocity.y < 0)
        {
            if (!grounded && Physics.Raycast(transform.position, Vector3.down, groundCheckForJump))
            {
                if (animator)
                    animator.SetTrigger("Fall");
            }
        }

        if (grounded && velocity.y < -.5f)
        {
            velocity.y = -.5f;
        }


        velocity.y += gravity * Time.deltaTime;
        chCont.Move(velocity * Time.deltaTime);
    }
    protected void DashingMove()
    {
        if (dashingNow)
        {
            dashT += Time.deltaTime;
            transform.position = Vector3.Lerp(dashOrPos, dashMove, dashT / dashTime);

            if (dashT >= dashTime)
            {
                transform.position = dashMove;
                dashT = 0;
                dashingNow = false;
            }

            Physics.SyncTransforms();
        }
        if (dashReset)
        {
            dashResetT += Time.deltaTime;

            if (dashResetT >= dashResetTime)
            {
                dashResetT = 0;
                dashReset = false;
                canDash = true;
            }
        }
    }
    protected void StartKnockback(Vector3 posToKbFrom)
    {
        if (canKb)
        {
            canDoAnythingElse = false;

            canKb = false;
            knockbacking = true;
            recuperating = true;

            kbDir = new Vector3(transform.position.x - posToKbFrom.x, 0, transform.position.z - posToKbFrom.z).normalized;
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, kbDir, out hitInfo, knockbackDistance))
            {
                if (Vector3.Distance(hitInfo.point, transform.position) < Vector3.Distance(hitInfo.point, transform.position + transform.forward * .5f))
                    kbMove = transform.position;
                else
                    kbMove = hitInfo.point + transform.forward * 0.5f;
            }
            else
                kbMove = transform.position + kbDir * knockbackDistance;

            kbOrPos = transform.position;
        }
    }
    protected void Knockback()
    {
        if (knockbacking)
        {
            kbT += Time.deltaTime;

            transform.position = Vector3.Lerp(kbOrPos, kbMove, kbT / timeKnockbacking);

            if (kbT >= timeKnockbacking)
            {
                transform.position = kbMove;
                kbT = 0;
                knockbacking = false;
                canKb = true;
            }

            Physics.SyncTransforms();
        }
    }
    protected void StartAttackKnockback()
    {
        if (canAttKb)
        {
            canDoAnythingElse = false;

            canAttKb = false;
            attKbing = true;

            attKbDir = -transform.forward;

            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position + Vector3.up, attKbDir, out hitInfo, attackKbDistance))
            {
                if (Vector3.Distance(hitInfo.point - Vector3.up, transform.position) < Vector3.Distance(hitInfo.point - Vector3.up, transform.position - transform.forward * .5f))
                    attKbMove = transform.position;
                else
                    attKbMove = hitInfo.point - Vector3.up + transform.forward * 0.5f;
            }
            else
                attKbMove = transform.position + attKbDir * attackKbDistance;

            attKbOrPos = transform.position;
        }
    }
    protected void AttackKnockback()
    {
        if (attKbing)
        {
            attKbT += Time.deltaTime;

            transform.position = Vector3.Lerp(attKbOrPos, attKbMove, attKbT / attackKbTime);

            if (attKbT >= attackKbTime)
            {
                transform.position = attKbMove;
                attKbT = 0;
                attKbing = false;
                canAttKb = true;
                canDoAnythingElse = true;
            }

            Physics.SyncTransforms();
        }
    }
    #endregion
    #region OTHER INPUT ACTIONS
    protected void Jump()
    {
        if (/*grounded && */canJump)
        {
            canAttack = false;
            velocity.y += Mathf.Sqrt(jumpHeight * -3f * gravity);
            canJump = false;
            if (animator)
                animator.SetTrigger("Jump");
        }
    }
    protected void EndJump()
    {
        canJump = true;
        canAttack = true;
    }
    protected void Dash()
    {
        if (canDash)
        {
            canDash = false;
            dashReset = true;
            dashingNow = true;

            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, transform.forward, out hitInfo, dashLenght))
            {
                if (Vector3.Distance(hitInfo.point, transform.position) < Vector3.Distance(hitInfo.point, transform.position + transform.forward * .5f))
                    dashMove = transform.position;
                else
                    dashMove = hitInfo.point - transform.forward * 0.5f;
            }
            else
                dashMove = transform.position + transform.forward * dashLenght;

            dashOrPos = transform.position;
        }
    }
    protected void Attack2()
    {
        canDoAnythingElse = false;
        animator.SetTrigger("Attack2");

    }

    protected void Attack1()
    {
        canDoAnythingElse = false;
        animator.SetTrigger("Attack1");

    }

    protected void Attack1OnMove()
    {
        canDoAnythingElse = false;
        animator.SetTrigger("Attack1OnMove");
    }

    protected virtual void ResetActions()
    {
        canDoAnythingElse = true;
    }

    
    #endregion
    #region VFX ANIM EVENTS
    protected virtual void StartAttack()
    {
        for(int i =0;i< RefsManager.I.Vfx_chargeAttack.Length; i++)
        {
            RefsManager.I.Vfx_chargeAttack[i].Play();

        }
    }

    protected virtual void MidAttack()
    {
        for (int i = 0; i < RefsManager.I.Vfx_projectile.Length; i++)
        {
            RefsManager.I.Vfx_projectile[i].Play();
        }
    }

    protected virtual void EndAttack()
    {
        for (int i = 0; i < RefsManager.I.Vfx_releaseAttack.Length; i++)
        {
            RefsManager.I.Vfx_releaseAttack[i].Play();
            RefsManager.I.Vfx_impact[i].Play();
        }
    }

    protected virtual void StartAttack2()
    {
        RefsManager.I.Vfx_Attack2ForwardSimpleEffect.SetTrigger("Trail");
        RefsManager.I.Vfx_Attack22ForwardSimpleEffect.SetTrigger("Trail");
    }
    
    [Header("VFX_POINTER REFS")]

    [SerializeField] private Transform oldParentProjectile;
    [SerializeField] private Transform oldParentImpact;
    [SerializeField] private Transform projectileTransform;
    [SerializeField] private Transform impactTransform;

    private Transform projectileOrLocal, impactOrLocal;
    private Vector3 proLocalPos, proLocalRot, impLocalPos, impLocalRot;

    protected virtual void UnparentAnimEvent()
    {
        proLocalPos = projectileTransform.localPosition;
        proLocalRot = projectileTransform.localEulerAngles;
        impLocalPos = impactTransform.localPosition;
        impLocalRot = impactTransform.localEulerAngles;
        projectileTransform.SetParent(null);
        impactTransform.SetParent(null);
    }

    protected virtual void ParentAnimEvent()
    {
        projectileTransform.gameObject.SetActive(false);
        impactTransform.gameObject.SetActive(false);
        projectileTransform.SetParent(oldParentProjectile);
        impactTransform.SetParent(oldParentImpact);

        projectileTransform.localPosition = proLocalPos;
        projectileTransform.localEulerAngles = proLocalRot;
        impactTransform.localPosition = impLocalPos;
        impactTransform.localEulerAngles = impLocalRot;
    }

    protected virtual void ReactivateVFX()
    {
        projectileTransform.gameObject.SetActive(true);
        impactTransform.gameObject.SetActive(true);
        //projectileTransform = projectileOrLocal;
        //impactTransform = impactOrLocal;
    }

    protected virtual void ActivateAttack1Collider()
    {
        attack1Collider.enabled = true;
    }

    protected virtual void DeactivateAttack1Collider()
    {
        attack1Collider.enabled = false;
    }

    #endregion
    #region TRIGGERS
    protected virtual void OnTriggerEnter(Collider other)
    {
        ButtonData button = other.GetComponent<ButtonData>();

        if (button)
        {
            if (!button.HasToBeAttacked)
                button.TogglePress(true);
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        ButtonData button = other.GetComponent<ButtonData>();

        if (button)
        {
            if (!button.HasToBeAttacked)
                button.TogglePress(false);
        }
    }
    #endregion
}
