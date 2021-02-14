using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float xMoveSpeed = 16f / 3f, zMoveSpeed = 3f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController chCont;
    private Vector3 velocity;
    private bool forward, left, back, right;
    private bool grounded;

    private void Awake()
    {
        chCont = GetComponent<CharacterController>();
    }
    private void Start()
    {
        ReferencesManager.Instance.InputRecAndPlay.I_Forward += SetForward;
        ReferencesManager.Instance.InputRecAndPlay.I_Back += SetBack;
        ReferencesManager.Instance.InputRecAndPlay.I_Left += SetLeft;
        ReferencesManager.Instance.InputRecAndPlay.I_Right += SetRight;
        ReferencesManager.Instance.InputRecAndPlay.I_Jump += Jump;
        ReferencesManager.Instance.InputRecAndPlay.I_Reset += ResetInputActions;
    }

    private void Update()
    {
        grounded = chCont.isGrounded;

        if (grounded && velocity.y < 0)
        {
            velocity.y = -.5f;
        }

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

        velocity.y += gravity * Time.deltaTime;
        chCont.Move(velocity * Time.deltaTime);
    }

    private void Jump()
    {
        if (grounded)
            velocity.y += Mathf.Sqrt(jumpHeight * -3f * gravity);
    }
    private void SetForward(bool tr) { forward = tr; }
    private void SetBack(bool tr) { back = tr; }
    private void SetLeft(bool tr) { left = tr; }
    private void SetRight(bool tr) { right = tr; }
    private void ResetInputActions() { forward = back = left = right = false; }
}
