using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool opening = false;
    [SerializeField] private float distanceToMove = 5f, speed = 3f;
    private float orPosX;
    private void Start()
    {
        orPosX = transform.position.x;
    }
    private void Update()
    {
        if (opening)
        {
            if (transform.position.x < orPosX + distanceToMove)
            {
                transform.position = transform.position + transform.right * speed * Time.deltaTime;
                //transform.Translate(transform.right * speed * Time.deltaTime);
                Physics.SyncTransforms();
            }
        }
        else
        {
            if (transform.position.x > orPosX)
            {
                transform.position = transform.position + transform.right * -speed * Time.deltaTime;
                //transform.Translate(transform.right * -speed * Time.deltaTime);
                Physics.SyncTransforms();
            }
        }
    }
}
