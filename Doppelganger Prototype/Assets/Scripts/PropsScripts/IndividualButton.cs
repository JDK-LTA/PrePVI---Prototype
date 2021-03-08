using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualButton : MonoBehaviour
{
    [SerializeField] private bool onlyPressOnce = false;
    [SerializeField] private bool hasToBeAttacked = false;
    [SerializeField] private Material pressedMat;

    private Material orMat;
    private MeshRenderer meshRenderer;

    private bool hasBeenPressed = false;

    public bool HasToBeAttacked { get => hasToBeAttacked; }
    public bool HasBeenPressed { get => hasBeenPressed; set => hasBeenPressed = value; }

    private ButtonsFunctionality buttonParent;

    private void Awake()
    {
        buttonParent = GetComponentInParent<ButtonsFunctionality>();
        meshRenderer = GetComponent<MeshRenderer>();
        if (!meshRenderer)
            meshRenderer = GetComponentInChildren<MeshRenderer>();
        orMat = meshRenderer.materials[0];
    }

    public void TogglePress(bool press)
    {
        if (!onlyPressOnce)
        {
            hasBeenPressed = press;
        }
        else
        {
            hasBeenPressed = true;
        }

        UpdateMat();

        buttonParent.CheckIfAllButtons();
    }

    private void UpdateMat()
    {
        meshRenderer.material = hasBeenPressed ? pressedMat : orMat;
    }

    public void SetToFalse()
    {
        hasBeenPressed = false;
        UpdateMat();
    }
}
