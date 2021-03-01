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
    protected Transform target;
    protected bool doppelInRange = false;
    protected bool playerInRange = false;

    public bool PlayerInRange { get => playerInRange; set => playerInRange = value; }
    public bool DoppelInRange { get => doppelInRange; set => doppelInRange = value; }

    protected virtual void Start()
    {
        target = RefsManager.I.PlayerCharacter.transform;
    }
    protected virtual void Attack() { }

    public virtual void UpdateTarget()
    {
        if (playerInRange)
        {
            target = RefsManager.I.PlayerCharacter.transform;
        }
        if (doppelInRange)
        {
            target = RefsManager.I.DoppelCharacter.transform;
        }
    }
}
