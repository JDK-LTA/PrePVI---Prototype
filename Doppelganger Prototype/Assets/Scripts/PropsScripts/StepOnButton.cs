using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepOnButton : MonoBehaviour
{
    private ButtonData[] buttons;
    private void Start()
    {
        buttons = GetComponentsInChildren<ButtonData>();
    }

    public bool CheckIfAllButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (!buttons[i].HasBeenPressed)
                return false;
        }
        print("yay");
        return true;
    }
}
