using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarnivoreAttackChecker : MonoBehaviour
{
    [SerializeField] private EnemyCarnivore carnivore;
    private bool shouldAttack = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerCharacter>() || other.GetComponent<DoppelCharacter>())
        {
            if (carnivore.CanAttack)
            {
                carnivore.Target = other.transform;
                carnivore.PublicAttack();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerCharacter>())
        {
            carnivore.PlayerInRange = false;
            carnivore.UpdateTarget();
        }
        else if (other.GetComponent<DoppelCharacter>())
        {
            carnivore.DoppelInRange = false;
            carnivore.UpdateTarget();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (carnivore.CanAttack)
        {
            if (other.GetComponent<PlayerCharacter>() || other.GetComponent<DoppelCharacter>())
            {
                carnivore.Target = other.transform;
                carnivore.PublicAttack();
            }
        }
    }
}
