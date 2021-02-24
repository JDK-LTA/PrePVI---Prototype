using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovement : MonoBehaviour
{
    [SerializeField] protected float xMoveSpeed = 16f / 3f, zMoveSpeed = 3f;
    [SerializeField] protected float jumpHeight = 1.5f;
    [SerializeField] protected float gravity = -9.81f;

    protected CharacterController chCont;
    protected Vector3 velocity;
    protected bool forward, left, back, right;
    protected bool grounded;

    protected void Awake()
    {
        chCont = GetComponent<CharacterController>();
    }
    
    protected void Update()
    {
        XZMove();
        YMove();
    }

    protected void YMove()
    {
        grounded = chCont.isGrounded;

        if (grounded && velocity.y < 0)
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

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
    }
    protected void Jump()
    {
        if (grounded)
            velocity.y += Mathf.Sqrt(jumpHeight * -3f * gravity);
    }

    protected void SetForward(bool tr) { forward = tr; }
    protected void SetBack(bool tr) { back = tr; }
    protected void SetLeft(bool tr) { left = tr; }
    protected void SetRight(bool tr) { right = tr; }
    protected void ResetInputActions() { forward = back = left = right = false; }

    public void MainInputSubscribing(bool subbing)
    {
        //if (subbing)
        //{
        //    ReferencesManager.I.InputRecAndPlay.I_Forward += SetForward;
        //    ReferencesManager.I.InputRecAndPlay.I_Back += SetBack;
        //    ReferencesManager.I.InputRecAndPlay.I_Left += SetLeft;
        //    ReferencesManager.I.InputRecAndPlay.I_Right += SetRight;
        //    ReferencesManager.I.InputRecAndPlay.I_Jump += Jump;
        //    //ReferencesManager.I.InputRecAndPlay.I_Reset += ResetInputActions;
        //}
        //else
        //{
        //    ReferencesManager.I.InputRecAndPlay.I_Forward -= SetForward;
        //    ReferencesManager.I.InputRecAndPlay.I_Back -= SetBack;
        //    ReferencesManager.I.InputRecAndPlay.I_Left -= SetLeft;
        //    ReferencesManager.I.InputRecAndPlay.I_Right -= SetRight;
        //    ReferencesManager.I.InputRecAndPlay.I_Jump -= Jump;
        //    //ReferencesManager.I.InputRecAndPlay.I_Reset -= ResetInputActions;
        //}
    }
}
