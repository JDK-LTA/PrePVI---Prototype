using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCactus : EnemyBase
{
    [Header("ANIMATION SETTINGS")]
    [SerializeField] protected float interpolationSpeed = 3f;

    private float animSpeed = 0;



    private bool followTarget = false, canMove = true;
    private Vector3 initialPos = Vector3.zero, initialRot;
    
    [SerializeField] private SphereCollider attackTrigger1;
    [SerializeField] private SphereCollider attackTrigger2;

    private NavMeshAgent agent;

    public bool FollowTarget { get => followTarget; set => followTarget = value; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    protected override void Start()
    {
        base.Start();
        initialPos = transform.position;
        initialRot = transform.rotation.eulerAngles;
        agent.speed = speed;
    }
    private void Update()
    {
        FollowAndAttack();
        AttackAnticipation();
        MoveAnimBlend();

        if (!canAttack)
        {
            attT += Time.deltaTime;
            if (attT >= cooldownToAttack)
            {
                attT = 0;
                canAttack = true;
            }
        }
    }

    private void MoveAnimBlend()
    {
        float agentSpeed = agent.velocity.magnitude;
        animSpeed = Mathf.MoveTowards(animSpeed, agentSpeed, interpolationSpeed * Time.deltaTime);
        animator.SetFloat("Blend", animSpeed);
    }

    private void FollowAndAttack()
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
                if (!readyToAttack && canAttack)
                {
                    readyToAttack = true;
                    canMove = false;
                    agent.isStopped = true;
                    anticipating = true;
                    canAttack = false;
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
    private void AE_DeactivateAttackTrigger1()
    {
        attackTrigger1.enabled=false;
    }
    private void AE_ActivateAttackTrigger1()
    {
        attackTrigger1.enabled = true;
    }
    private void AE_DeactivateAttackTrigger2()
    {
        attackTrigger2.enabled = false;
    }
    private void AE_ActivateAttackTrigger2()
    {
        attackTrigger2.enabled = true;
    }


}
