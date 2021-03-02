using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCactus : EnemyBase
{
    [SerializeField] private float radiusToStartAttack = 1.2f;
    [SerializeField] private float radiusToKeepAttacking = 1.6f;
    [SerializeField] private float slowRadius = 3f;
    [SerializeField] private float distanceToBack = 15f;
    [SerializeField] protected float attackAnticipationTime = 0.8f;

    private float anticT = 0f;
    private bool anticipating = false;

    private bool followTarget = false, canMove = true;
    private Vector3 initialPos = Vector3.zero, initialRot;
    
    private SphereCollider attackTrigger;

    private NavMeshAgent agent;

    public bool FollowTarget { get => followTarget; set => followTarget = value; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        attackTrigger = GetComponentInChildren<SphereCollider>();
    }
    protected override void Start()
    {
        base.Start();
        initialPos = transform.position;
        initialRot = transform.rotation.eulerAngles;
    }
    private void Update()
    {
        if (followTarget)
        {
            if (canMove && Vector3.Distance(transform.position, target.position) > radiusToStartAttack)
            {
                agent.isStopped = !canMove;
                agent.SetDestination(target.position);
            }
            else
            {
                if (!readyToAttack)
                {
                    readyToAttack = true;
                    canMove = false;
                    agent.isStopped = true;
                    animator.SetTrigger("Attack");
                }
            }

        }
        else
        {
            if (Vector3.Distance(transform.position, initialPos) > radiusToStartAttack)
                agent.SetDestination(initialPos);
            else
            {
                transform.position = initialPos;
                transform.eulerAngles = initialRot;
            }
        }

        if (anticipating)
        {
            anticT += Time.deltaTime;
            if (anticT >= attackAnticipationTime)
            {
                animator.SetTrigger("FinishAttack");
            }
        }
    }

    public override void UpdateTarget()
    {
        base.UpdateTarget();
        if (!playerInRange && !doppelInRange)
        {
            followTarget = false;
        }
    }
    private void AE_EndAttack()
    {
        canMove = true;
        readyToAttack = false;
    }
    private void AE_DeactivateAttackTrigger()
    {
        attackTrigger.gameObject.SetActive(false);
    }
    private void AE_ActivateAttackTrigger()
    {
        attackTrigger.gameObject.SetActive(true);
    }
}
