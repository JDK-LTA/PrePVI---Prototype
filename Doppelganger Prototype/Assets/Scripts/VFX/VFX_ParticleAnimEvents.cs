using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_ParticleAnimEvents : MonoBehaviour
{
    protected virtual void StartAttack1Particles()
    {
        RefsManager.I.Vfx_Attack1ParticlesUp.enabled = true;
        RefsManager.I.Vfx_Attack1ParticlesUp.Play();
    }

    protected virtual void EndAttack1Particles()
    {
        RefsManager.I.Vfx_Attack1ParticlesUp.Stop();
    }

    protected virtual void StartAttack2Particles()
    {
        RefsManager.I.Vfx_Attack2ParticlesUp.enabled = true;
        RefsManager.I.Vfx_Attack2ParticlesUp.Play();
    }

    protected virtual void EndAttack2Particles()
    {
        RefsManager.I.Vfx_Attack2ParticlesUp.Stop();
    }

    protected virtual void StartAttack22Particles()
    {
        RefsManager.I.Vfx_Attack22ParticlesUp.enabled = true;
        RefsManager.I.Vfx_Attack22ParticlesUp.Play();
    }

    protected virtual void EndAttack22Particles()
    {
        RefsManager.I.Vfx_Attack22ParticlesUp.Stop();
    }
}
