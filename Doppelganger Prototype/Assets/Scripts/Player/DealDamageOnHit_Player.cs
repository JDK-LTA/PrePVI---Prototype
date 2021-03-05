using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageOnHit_Player:MonoBehaviour
{
    [SerializeField] private float playerDamage = 10.0f;

    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = other.GetComponent<EnemyBase>();
        if (enemy)
        {
            enemy.ApplyDamage(playerDamage);
        }
    }
}
