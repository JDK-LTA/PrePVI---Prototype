using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BinaryInputs { DASH, JUMP, ATTACK1, ATTACK2 }
[RequireComponent(typeof(CharacterController))]
public class BaseCharacter : MonoBehaviour
{
    [SerializeField] protected KeyCode keyToRecord = KeyCode.R;
    [SerializeField] protected KeyCode keyToPlayback = KeyCode.P;
    [SerializeField] protected KeyCode keyToForward = KeyCode.W;
    [SerializeField] protected KeyCode keyToLeft = KeyCode.A;
    [SerializeField] protected KeyCode keyToBack = KeyCode.S;
    [SerializeField] protected KeyCode keyToRight = KeyCode.D;
    [SerializeField] protected KeyCode keyToJump = KeyCode.Space;

    [SerializeField] protected float xMoveSpeed = 16f / 3f, zMoveSpeed = 3f;
    [SerializeField] protected float jumpHeight = 1.5f;
    [SerializeField] protected float groundCheckDistance = 0.12f;
    [SerializeField] protected float groundCheckForJump = 0.4f;
    [SerializeField] protected float gravity = -9.81f;

    [SerializeField] protected float dashLenght = 0.15f;
    [SerializeField] protected float dashSpeed = 100f;
    [SerializeField] protected float dashResetTime = 1f;

    protected Vector3 dashMove;
    protected float dashing = 0f;
    protected float dashingTime = 0f;
    protected bool canDash = true;
    protected bool dashingNow = false;
    protected bool dashReset = true;

    protected CharacterController chCont;
    protected Animator animator;

    protected Vector2 xzInput;
    protected Vector3 velocity;
    protected bool forward, left, back, right;
    protected bool grounded;

    protected static bool rec = false;
    protected static bool canStartRecording = true;

    protected static bool[] wasdInput = new bool[4];
    public bool[] wasdDebug = new bool[4];

    protected void Awake()
    {
        chCont = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        //Inputs();
        wasdDebug = wasdInput;
        XZMove();
        YMove();
    }
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
        if (Input.GetButtonDown("Dash")/* && dashing < dashLenght && dashingTime < dashResetTime && dashReset && canDash*/)
        {
            print("Dash");
        }
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
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
    protected void Jump()
    {
        if (grounded)
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -3f * gravity);

            if (animator)
                animator.SetTrigger("Jump");
        }
    }

    protected void SetInputAxis(ref bool inp, int wasdIndex, bool setter)
    {
        inp = setter;
        if (wasdInput[wasdIndex] == !setter)
            wasdInput[wasdIndex] = setter;
    }
    protected void ResetInputActions() { forward = back = left = right = false; }
    public void ResetAndReinput()
    {
        ResetInputActions();
        forward = wasdInput[0];
        left = wasdInput[1];
        back = wasdInput[2];
        right = wasdInput[3];
    }

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
}
