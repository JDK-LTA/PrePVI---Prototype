using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : BaseCharacter
{
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