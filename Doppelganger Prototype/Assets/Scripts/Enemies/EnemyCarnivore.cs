using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCarnivore : EnemyBase
{
    [SerializeField] private Vector3 centerOfSpawnCircle = Vector3.zero;
    [SerializeField] private float radiusOfSpawnCircle = 16f;
    [SerializeField] private float minDistanceFromPlayerToSpawn = 8f;
    [SerializeField] private float timeFromDigToOut = 0.8f;

    private bool isDug = false;
    private float dugT = 0;

    private void Update()
    {
        AttackAnticipation();
        DigOutTimer();
    }

    protected override void Attack()
    {
        transform.LookAt(target);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        animator.SetTrigger("Attack");
        anticipating = true;
    }

    private void DigOutTimer()
    {
        if (isDug)
        {
            dugT += Time.deltaTime;
            if (dugT >= timeFromDigToOut)
            {
                dugT = 0;
                isDug = false;
                ChangePlace();
            }
        }
    }

    //Call on Animation Event
    private void ChangePlace()
    {
        Vector3 spawnPos;
        bool canSpawn = false;
        do
        {
            Vector2 ran = Random.insideUnitCircle;
            spawnPos = centerOfSpawnCircle + new Vector3(ran.x, 0, ran.y) * Random.Range(0f, radiusOfSpawnCircle);
            canSpawn = Physics.OverlapSphere(spawnPos + Vector3.up, 0).Length == 0;
        } while (Vector3.Distance(target.position, spawnPos) < minDistanceFromPlayerToSpawn && !canSpawn);

        transform.position = spawnPos;
        Physics.SyncTransforms();

        animator.SetTrigger("GoOut");
    }
}
