using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    protected CharacterController chCont;
    protected Animator animator;

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

    protected void Inputs()
    {
        //PRESSES
        if (Input.GetKeyDown(keyToForward))
        {
            forward = true;
            if (!wasdInput[0])
                wasdInput[0] = true;
        }
        if (Input.GetKeyDown(keyToLeft))
        {
            left = true;
            if (!wasdInput[1])
                wasdInput[1] = true;
        }
        if (Input.GetKeyDown(keyToBack))
        {
            back = true;
            if (!wasdInput[2])
                wasdInput[2] = true;
        }
        if (Input.GetKeyDown(keyToRight))
        {
            right = true;
            if (!wasdInput[3])
                wasdInput[3] = true;
        }
        if (Input.GetKeyDown(keyToJump))
        {
            Jump();
        }
        //------------------------------------------------

        //RELEASES
        if (Input.GetKeyUp(keyToForward))
        {
            forward = false;
            if (wasdInput[0])
                wasdInput[0] = false;
        }
        if (Input.GetKeyUp(keyToLeft))
        {
            left = false;
            if (wasdInput[1])
                wasdInput[1] = false;
        }
        if (Input.GetKeyUp(keyToBack))
        {
            back = false;
            if (wasdInput[2])
                wasdInput[2] = false;
        }
        if (Input.GetKeyUp(keyToRight))
        {
            right = false;
            if (wasdInput[3])
                wasdInput[3] = false;
        }
        //------------------------------------------------
    }
    protected void YMove()
    {
        if (velocity.y < 0)
        {
            grounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
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
    protected void XZMove()
    {
        int vertInv = 0, horInv = 0;
        if (forward)
            vertInv = 1;
        else if (back)
            vertInv = -1;
        if (right)
            horInv = 1;
        else if (left)
            horInv = -1;

        Vector3 move = new Vector3(horInv * xMoveSpeed, 0, vertInv * zMoveSpeed);
        chCont.Move(move * Time.deltaTime);

        if (animator)
            animator.SetFloat("MoveSpeed", Mathf.Max(Mathf.Abs(horInv), Mathf.Abs(vertInv)));

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
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
            button.TogglePress(false);
        }
    }
}
