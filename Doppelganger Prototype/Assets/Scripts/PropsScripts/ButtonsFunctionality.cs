using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsFunctionality : MonoBehaviour
{
    private IndividualButton[] buttons;
    private void Start()
    {
        buttons = GetComponentsInChildren<IndividualButton>();
    }

    public bool CheckIfAllButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (!buttons[i].HasBeenPressed)
                return false;
        }
        Action();
        return true;
    }

    protected virtual void Action() { }
}
