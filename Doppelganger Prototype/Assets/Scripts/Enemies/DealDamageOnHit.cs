using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageOnHit:MonoBehaviour
{
    [SerializeField] private float enemyDamage=5.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerCharacter>())
        {
            RefsManager.I.PlayerCharacter.ApplyDamage(enemyDamage);
        }
    }
}
