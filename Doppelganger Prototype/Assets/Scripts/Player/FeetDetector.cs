using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<BaseCharacter>())
            RefsManager.I.PlayerCharacter.Grounded = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<BaseCharacter>())
            RefsManager.I.PlayerCharacter.Grounded = false;
    }
}
