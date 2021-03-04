using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("HEALTH")]
    [SerializeField] protected float health;
    [SerializeField] protected bool canRegenerate = false;
    [SerializeField] protected float regenerationRate;
    [SerializeField] protected float timeIdleToStartRegen = 4f;
    [Header("BASE MOVEMENT")]
    [SerializeField] protected float speed;
    [Header("BASE ATTACKING")]
    [SerializeField] protected float baseDamage;
    [SerializeField] protected float cooldownToAttack;
    [SerializeField] protected float radiusToStartAttack = 1.2f;
    [SerializeField] protected float attackAnticipationTime = 0.8f;

    protected bool canAttack = true;
    protected bool readyToAttack = false;
    protected float attT = 0;
    protected Transform target;
    protected bool doppelInRange = false;
    protected bool playerInRange = false;

    protected Animator animator;

    public bool PlayerInRange { get => playerInRange; set => playerInRange = value; }
    public bool DoppelInRange { get => doppelInRange; set => doppelInRange = value; }

    protected virtual void Start()
    {
        target = RefsManager.I.PlayerCharacter.transform;
        animator = GetComponent<Animator>();
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
