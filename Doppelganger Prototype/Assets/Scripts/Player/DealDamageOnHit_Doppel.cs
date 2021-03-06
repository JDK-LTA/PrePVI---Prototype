using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageOnHit_Doppel : MonoBehaviour
{
    [SerializeField] private bool quickTrueCircleFalse = true;

    private DoppelCharacter doppelCh;
    private float dmg;

    private void Awake()
    {
        doppelCh = GetComponent<DoppelCharacter>();
        dmg = quickTrueCircleFalse ? doppelCh.Damage : doppelCh.Damage2;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = other.GetComponent<EnemyBase>();
        if (enemy && doppelCh.IsFree)
        {
            enemy.ApplyDamage(dmg);
        }
    }
}
