using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    [Header("HEALTH")]
    [SerializeField] protected float currentEnemyLife = 100.0f;
    [SerializeField] protected float maxEnemyLife = 100.0f;
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
    [Header("UI REFERENCES")]
    [SerializeField] protected Image healthBar;

    protected bool isIdle = false;
    protected float regenT = 0;

    protected bool canAttack = true;
    protected bool readyToAttack = false;
    protected float attT = 0;
    protected Transform target;
    protected bool doppelInRange = false;
    protected bool playerInRange = false;

    protected Animator animator;

    protected float anticT = 0f;
    protected bool anticipating = false;

    public bool PlayerInRange { get => playerInRange; set => playerInRange = value; }
    public bool DoppelInRange { get => doppelInRange; set => doppelInRange = value; }
    public bool CanAttack { get => canAttack; }
    public Transform Target { get => target; set => target = value; }
    public float BaseDamage { get => baseDamage; set => baseDamage = value; }

    protected virtual void Start()
    {
        target = RefsManager.I.PlayerCharacter.transform;
        animator = GetComponent<Animator>();
    }
    protected virtual void Update()
    {
        if (canRegenerate)
        {
            RegenTimer();
        }
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
    protected virtual void AttackAnticipation()
    {
        if (anticipating)
        {
            anticT += Time.deltaTime;
            if (anticT >= attackAnticipationTime)
            {
                animator.SetTrigger("FinishAttack");
                anticT = 0;
                anticipating = false;
            }
        }
    }

    protected virtual void RegenTimer()
    {
        if (isIdle)
        {
            if (regenT < timeIdleToStartRegen)
                regenT += Time.deltaTime;
            else if(currentEnemyLife < maxEnemyLife)
            {
                currentEnemyLife += regenerationRate * Time.deltaTime;
                if (currentEnemyLife > maxEnemyLife)
                {
                    currentEnemyLife = maxEnemyLife;
                }
            }
        }
    }
    public void ApplyDamage(float damage)
    {
        currentEnemyLife -= damage;
        currentEnemyLife = Mathf.Clamp(currentEnemyLife, 0, maxEnemyLife);
        UpdateHealthBar();

        if (currentEnemyLife == 0)
        {
            OnEnemyDead();
        }

    }

    protected void UpdateHealthBar()
    {
        healthBar.fillAmount = currentEnemyLife / maxEnemyLife;
    }

    protected void OnEnemyDead()
    {
        //Kill Enemy or recycle
    }
}
