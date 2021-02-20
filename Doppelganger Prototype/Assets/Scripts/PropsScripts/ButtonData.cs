using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonData : MonoBehaviour
{
    [SerializeField] private bool onlyPressOnce = false;
    [SerializeField] private bool hasToBeAttacked = false;
    private bool hasBeenPressed = false;

    public bool HasToBeAttacked { get => hasToBeAttacked; }
    public bool HasBeenPressed { get => hasBeenPressed; set => hasBeenPressed = value; }

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
