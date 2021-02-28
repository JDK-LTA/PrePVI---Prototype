using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected bool canRegenerate = false;
    [SerializeField] protected float regenerationRate;
    [SerializeField] protected float timeIdleToStartRegen = 4f;    
    [SerializeField] protected float speed;
    [SerializeField] protected float baseDamage;
    [SerializeField] protected float cooldownToAttack;

    protected bool canAttack = true;
    protected float attT = 0;


    protected virtual void Attack() { }
}
