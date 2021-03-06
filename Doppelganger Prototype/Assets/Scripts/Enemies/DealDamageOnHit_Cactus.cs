using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageOnHit_Cactus : MonoBehaviour
{
    [SerializeField] private EnemyCactus cactus;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerCharacter>())
        {
            RefsManager.I.PlayerCharacter.ApplyDamage(cactus.BaseDamage, transform.position);
        }
    }
}
