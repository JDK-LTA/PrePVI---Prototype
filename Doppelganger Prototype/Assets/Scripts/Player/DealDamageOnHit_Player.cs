using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageOnHit_Player : MonoBehaviour
{
    [SerializeField] private bool quickTrueCircleFalse = true;

    private PlayerCharacter playerCh;
    private float dmg;

    private void Awake()
    {
        playerCh = transform.root.GetComponentInParent<PlayerCharacter>();
        dmg = quickTrueCircleFalse ? playerCh.Damage : playerCh.Damage2;
    }
    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = other.GetComponent<EnemyBase>();
        if (enemy)
        {
            enemy.ApplyDamage(dmg);
        }
    }
}
