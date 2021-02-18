using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonData : MonoBehaviour
{
    [SerializeField] private bool onlyPressOnce = false;
    /*[HideInInspector]*/ public bool hasBeenPressed = false;

    public void TogglePress(bool press)
    {
        if(!onlyPressOnce)
        {
            hasBeenPressed = press;
        }
        else
        {
            hasBeenPressed = true;
        }
        ReferencesManager.I.ButtonSystem.CheckIfAllButtons();
    }
}
