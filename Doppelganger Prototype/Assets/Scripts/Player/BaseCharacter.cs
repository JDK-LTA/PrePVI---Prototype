using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BinaryInputs { DASH, JUMP, ATTACK1, ATTACK2 }
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

    [Header("Attack 1")]
    [SerializeField] protected float damage = 5f;
    [SerializeField] protected float timeToReachEnd = 0.5f;
    [SerializeField] protected float cooldown1 = 0.8f;

    protected CharacterController chCont;
    protected Animator animator;

    protected Vector2 xzInput;
    protected Vector3 velocity;

    protected bool grounded;

    protected Vector3 dashMove, dashOrPos;
    protected float dashT = 0f, dashResetT;
    protected bool canDash = true;
    protected bool dashingNow = false;
    protected bool dashReset = false;

    protected bool canAttack1 = true;

    protected bool canDoAnythingElse = true;
    protected float generalT = 0;

    protected static bool rec = false;
    protected static bool canStartRecording = true;

    #region BASIC MONOBEHAVIOUR METHODS
    protected void Awake()
    {
        chCont = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        XZMove();
        YMove();
        DashingMove();
    }
    #endregion
    #region INPUT
    protected void ContinousInput()
    {
        int aux = canDoAnythingElse ? 1 : 0;
        xzInput.x = Input.GetAxis("Horizontal") * aux;
        xzInput.y = Input.GetAxis("Vertical") * aux;
    }
    protected void BinInputs()
    {
        //PRESSES
        if (Input.GetButtonDown("Attack1") && canDoAnythingElse)
        {
            Attack1();
        }
        if (Input.GetButtonDown("Attack2") && canDoAnythingElse)
        {
            print("Attack 2");
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
        grounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
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
    #endregion
    #region OTHER INPUT ACTIONS
    protected void Jump()
    {
        if (grounded)
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -3f * gravity);

            if (animator)
                animator.SetTrigger("Jump");
        }
    }
    protected void Dash()
    {
        if (canDash)
        {
            canDash = false;
            dashReset = true;
            dashingNow = true;
            dashMove = transform.position + transform.forward * dashLenght;
            dashOrPos = transform.position;
        }
    }
    protected void Attack1()
    {
        //canDoAnythingElse = false;
        
    }
    protected void StartAttack1()
    {

    }
    #endregion
    #region TRIGGERS
    protected virtual void OnTriggerEnter(Collider other)
    {
        ButtonData button = other.GetComponent<ButtonData>();

        if (button)
        {
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
