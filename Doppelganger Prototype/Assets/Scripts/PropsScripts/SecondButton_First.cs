using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondButton_First : MonoBehaviour
{
    [SerializeField] private Door door;
    [SerializeField] private Material pressedMat;
    private Material orMat;
    private MeshRenderer meshRenderer;
    private bool playerOn = false, doppelOn = false;

    private void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        orMat = meshRenderer.materials[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        BaseCharacter bc = other.GetComponent<BaseCharacter>();
        if (bc)
        {
            door.opening = true;
            meshRenderer.material = pressedMat;

            if (bc is PlayerCharacter)
            {
                playerOn = true;
            }
            else
            {
                doppelOn = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        BaseCharacter bc = other.GetComponent<BaseCharacter>();
        if (bc)
        {
            if (bc is PlayerCharacter)
            {
                playerOn = false;
            }
            else
            {
                doppelOn = false;
            }

            if (!playerOn && !doppelOn)
            {
                door.opening = false;
                meshRenderer.material = orMat;
            }
        }
    }
}
