using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseFirstDoor : MonoBehaviour
{
    [SerializeField] private Animator leftDoorAnim, rightDoorAnim;
    private bool hasBeenClosed = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerCharacter>() && !hasBeenClosed)
        {
            hasBeenClosed = true;
            leftDoorAnim.SetTrigger("Close");
            rightDoorAnim.SetTrigger("Close");
        }
    }
}
