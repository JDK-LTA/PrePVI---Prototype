using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : BaseCharacter
{
    [Header("Player Stats")]
    [SerializeField] private float currentLife = 100.0f;
    [SerializeField] private float maxLife = 100.0f;
    [SerializeField] private float timeKnockbacking = 0.4f;
    [SerializeField] private float knockbackDistance = 0.5f;

    private bool knockbacking = false, canKb = true;
    private float kbT = 0;
    private Vector3 kbOrPos, kbMove, kbDir;

    protected override void Update()
    {
        if (canStartRecording && Input.GetButtonDown("Doppel"))
        {
            RefsManager.I.DoppelCharacter.gameObject.SetActive(true);
            RefsManager.I.DoppelCharacter.StartRecording();
            CamerasManager.I.ToggleSingleDoppelCams(true);

            animator.SetFloat("MoveSpeed", 0);
            animator.enabled = false;

            rec = true;
            enabled = false;

            RefsManager.I.ParticleChainGO.gameObject.SetActive(true);
            RefsManager.I.ParticleChainGO.playRate = 2;

            //VFX Hologram Activate call
            for (int i = 0; i < RefsManager.I.Vfx_HoloParticles.Length; i++)
            {
                RefsManager.I.Vfx_HoloParticles[i].enabled = true;
            }
        }

        if (!rec)
            BinInputs();



        base.Update();
    }

    public void ApplyDamage(float damage, Vector3 enemyPos)
    {
        currentLife -= damage;

        StartKnockback(enemyPos);

        if (currentLife <= 0)
        {
            currentLife = 0;
            UpdateHealthBar();
            OnPlayerDead();
        }

        UpdateHealthBar();
    }

    private void StartKnockback(Vector3 enemyPos)
    {
        if (canKb)
        {
            canDoAnythingElse = false;

            canKb = false;
            knockbacking = true;
            kbDir = (transform.position - enemyPos).normalized;
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, kbDir, out hitInfo, knockbackDistance))
            {
                if (Vector3.Distance(hitInfo.point, transform.position) < Vector3.Distance(hitInfo.point, transform.position + transform.forward * .5f))
                    kbMove = transform.position;
                else
                    kbMove = hitInfo.point - transform.forward * 0.5f;
            }
            else
                kbMove = transform.position + kbDir * knockbackDistance;

            kbOrPos = transform.position;
        }
    }

    private void Knockback()
    {
        if (knockbacking)
        {
            kbT += Time.deltaTime;

            transform.position = Vector3.Lerp(kbOrPos, kbMove, kbT / timeKnockbacking);

            if (kbT >= timeKnockbacking)
            {
                transform.position = kbMove;
                kbT = 0;
                knockbacking = false;
                canKb = true;
                canDoAnythingElse = true;
            }

            Physics.SyncTransforms();
        }
    }
    private void UpdateHealthBar()
    {
        RefsManager.I.Player_LifeBar.fillAmount = currentLife / maxLife;
    }

    private void OnPlayerDead()
    {
        //Reload Level
    }

    private void FixedUpdate()
    {
        if (!rec)
        {
            ContinousInput();
            //VFX Hologram Deactivate call
            for (int i = 0; i < RefsManager.I.Vfx_HoloParticles.Length; i++)
            {
                RefsManager.I.Vfx_HoloParticles[i].enabled = false;
            }
        }
    }
}