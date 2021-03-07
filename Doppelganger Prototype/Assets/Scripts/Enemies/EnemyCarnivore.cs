using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCarnivore : EnemyBase
{
    [SerializeField] private Vector3 centerOfSpawnCircle = Vector3.zero;
    [SerializeField] private float radiusOfSpawnCircle = 16f;
    [SerializeField] private float minDistanceFromPlayerToSpawn = 8f;
    [SerializeField] private float timeFromDigToOut = 0.8f;
    [SerializeField] private Collider attackCol;

    private bool isDug = false;
    private float dugT = 0;

    protected override void Update()
    {
        base.Update();

        AttackAnticipation();
        DigOutTimer();
        OnCarnivoreDead();
    }

    protected override void Attack()
    {
        transform.LookAt(target);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        Physics.SyncTransforms();

        animator.SetTrigger("Attack");
        anticipating = true;
        canAttack = false;
    }
    public void PublicAttack()
    {
        if (canAttack)
            Attack();
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
    private void Dig() { isDug = true; }
    private void CanAttackAgain() { canAttack = true; }
    private void DeactivateAttackTrigger() { attackCol.enabled = false; }
    private void ReactivateAttackTrigger() { attackCol.enabled = true; }
    //__________________________
    private void ChangePlace()
    {
        Vector3 spawnPos;
        bool canSpawn = false;
        do
        {
            Vector2 ran = Random.insideUnitCircle;
            spawnPos = centerOfSpawnCircle + new Vector3(ran.x, 0, ran.y) * Random.Range(0f, radiusOfSpawnCircle);
            canSpawn = Physics.OverlapSphere(spawnPos + Vector3.up, 1).Length == 0;
            //spawnPos.y = transform.position.y;
        } while (Vector3.Distance(target.position, spawnPos) < minDistanceFromPlayerToSpawn && !canSpawn);

        transform.position = spawnPos;
        Physics.SyncTransforms();

        animator.SetTrigger("GoOut");
    }

    private void OnCarnivoreDead()
    {
        if (currentEnemyLife <= 0)
        {
            RefsManager.I.Vfx_enemyCarnivoreDeath.Play();
            Invoke("DelayDead", 1.0f);
        }
    }

    public void DelayDead()
    {
        GameObject.Destroy(gameObject);
    }
}
