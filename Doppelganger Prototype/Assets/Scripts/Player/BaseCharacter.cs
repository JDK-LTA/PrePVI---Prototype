using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BinaryInputs { DASH, JUMP, ATTACK1, ATTACK2 }
[RequireComponent(typeof(CharacterController))]
public class BaseCharacter : MonoBehaviour
{
    [SerializeField] protected float xMoveSpeed = 16f / 3f, zMoveSpeed = 16f / 3f;
    [SerializeField] protected float jumpHeight = 1.5f;
    [SerializeField] protected float groundCheckDistance = 0.12f;
    [SerializeField] protected float groundCheckForJump = 0.4f;
    [SerializeField] protected float gravity = -9.81f;

    [SerializeField] protected float dashLenght = 4f;
    [SerializeField] protected float dashTime = 0.2f;
    [SerializeField] protected float dashResetTime = 1f;

    protected Vector3 dashMove, dashOrPos;
    protected float dashT = 0f, dashResetT;
    protected bool canDash = true;
    protected bool dashingNow = false;
    protected bool dashReset = false;

    protected CharacterController chCont;
    protected Animator animator;

    protected Vector2 xzInput;
    protected Vector3 velocity;
    protected bool forward, left, back, right;
    protected bool grounded;

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
        xzInput.x = Input.GetAxis("Horizontal");
        xzInput.y = Input.GetAxis("Vertical");
    }
    protected void BinInputs()
    {
        //PRESSES
        if (Input.GetButtonDown("Attack1"))
        {
            print("Attack 1");
        }
        if (Input.GetButtonDown("Attack2"))
        {
            print("Attack 2");
        }
        if (Input.GetButtonDown("Dash") && canDash)
        {
            Dash();
        }
        if (Input.GetButtonDown("Jump"))
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
            animator.SetFloat("MoveSpeed", Mathf.Clamp01(Mathf.Abs(xzInput.x + xzInput.y)));

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
        canDash = false;
        dashReset = true;
        dashingNow = true;
        dashMove = transform.position + transform.forward * dashLenght;
        dashOrPos = transform.position;
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
