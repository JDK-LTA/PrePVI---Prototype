using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstDoorButtonsFunct : ButtonsFunctionality
{
    [SerializeField] private Animator leftDoorAnim, rightDoorAnim;
    protected override void Action()
    {
        leftDoorAnim.SetTrigger("Open");
        rightDoorAnim.SetTrigger("Open");
    }
}
