using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : BaseMovement
{

    private Vector3 originalPos = Vector3.up;

    public Vector3 OriginalPos { get => originalPos; set => originalPos = value; }


    private void Start()
    {
        MainInputSubscribing(true);
    }
}
