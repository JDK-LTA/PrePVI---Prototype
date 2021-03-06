using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageOnHit : MonoBehaviour
{
    [SerializeField] private EnemyBase enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerCharacter>())
        {
            RefsManager.I.PlayerCharacter.ApplyDamage(enemy.BaseDamage, transform.position);
        }
    }
}
