using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreDoppelTime : MonoBehaviour
{
    [SerializeField] private bool moreTrueLessFalse = true;
    private float orTime;
    private void Start()
    {
        orTime = RefsManager.I.DoppelCharacter.TimeRecording;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerCharacter>())
        {
            if (moreTrueLessFalse)
                RefsManager.I.DoppelCharacter.TimeRecording = 6f;
            else
                RefsManager.I.DoppelCharacter.TimeRecording = orTime;
        }
    }
}
