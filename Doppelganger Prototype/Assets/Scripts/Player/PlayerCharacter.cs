using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : BaseCharacter
{
    [Header("Player Stats")]
    [SerializeField] private float currentLife = 100.0f;
    [SerializeField] private float maxLife = 100.0f;

    

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
            for(int i=0;i< RefsManager.I.Vfx_HoloParticles.Length; i++)
            {
                RefsManager.I.Vfx_HoloParticles[i].enabled = true;
            }
        }

        if (!rec)
            BinInputs();

        base.Update();
    }
    
    public void ApplyDamage(float damage)
    {
        currentLife -= damage;
        
        if (currentLife <= 0)
        {
            currentLife = 0;
            UpdateHealthBar();
            OnPlayerDead();
        }

        UpdateHealthBar();
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