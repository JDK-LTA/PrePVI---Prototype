using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCactus : EnemyBase
{
    [SerializeField] private float radiusToStartAttack = 0.4f;
    [SerializeField] private float radiusToKeepAttacking = 0.6f;
    [SerializeField] private float slowRadius = 1.5f;
    [SerializeField] private float distanceToBack = 15f;

    private bool followTarget = false;
    private Vector3 initialPos = Vector3.zero;

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
    }
    private void Update()
    {
        if (followTarget && ShouldFollow())
        {
            //agent
        }
    }

    private bool ShouldFollow()
    {
        if (Vector3.Distance(transform.position, initialPos) > distanceToBack)
        {
            return false;
        }


        return true;
    }
    protected override void Attack()
    {

    }
    public override void UpdateTarget()
    {
        base.UpdateTarget();
        if (!playerInRange && !doppelInRange)
        {
            followTarget = false;
        }
    }
}
