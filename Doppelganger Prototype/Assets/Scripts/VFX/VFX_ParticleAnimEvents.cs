using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_ParticleAnimEvents : MonoBehaviour
{
    [SerializeField] private bool isDoppel = false;
    protected virtual void StartAttack1Particles()
    {
        if (!isDoppel)
        {
            RefsManager.I.Vfx_Attack1ParticlesUp.enabled = true;
            RefsManager.I.Vfx_Attack1ParticlesUp.Play();
        }
    }

    protected virtual void EndAttack1Particles()
    {
        if (!isDoppel)
        {
            RefsManager.I.Vfx_Attack1ParticlesUp.Stop();
        }
    }

    protected virtual void StartAttack2Particles()
    {
        if (!isDoppel)
        {
            RefsManager.I.Vfx_Attack2ParticlesUp.enabled = true;
            RefsManager.I.Vfx_Attack2ParticlesUp.Play();
        }
        else
        {
            RefsManager.I.Vfx_Attack2ParticlesUpDoppel.enabled = true;
            RefsManager.I.Vfx_Attack2ParticlesUpDoppel.Play();
        }
    }

    protected virtual void EndAttack2Particles()
    {
        if (!isDoppel)
        {
            RefsManager.I.Vfx_Attack2ParticlesUp.Stop();
        }
        else
        {
            RefsManager.I.Vfx_Attack2ParticlesUpDoppel.Stop();
        }
    }

    protected virtual void StartAttack22Particles()
    {
        if (!isDoppel)
        {
            RefsManager.I.Vfx_Attack22ParticlesUp.enabled = true;
            RefsManager.I.Vfx_Attack22ParticlesUp.Play();
        }
        else
        {
            RefsManager.I.Vfx_Attack22ParticlesUpDoppel.enabled = true;
            RefsManager.I.Vfx_Attack22ParticlesUpDoppel.Play();
        }
    }

    protected virtual void EndAttack22Particles()
    {
        if (!isDoppel)
        {
            RefsManager.I.Vfx_Attack22ParticlesUp.Stop();
        }
        else
        {
            RefsManager.I.Vfx_Attack22ParticlesUpDoppel.Stop();
        }
    }
}
