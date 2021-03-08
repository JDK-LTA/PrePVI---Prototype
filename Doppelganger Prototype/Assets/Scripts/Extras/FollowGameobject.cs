using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGameobject : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool followRotation=false;
    // Update is called once per frame
    private void Update()
    {
        if (followRotation)
        {
            transform.rotation = target.rotation;
        }
        transform.position = target.position + offset;
    }
}
