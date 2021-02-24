using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : BaseCharacter
{
    protected override void Update()
    {
        if (canStartRecording && Input.GetKeyDown(keyToRecord))
        {
            RefsManager.I.DoppelCharacter.gameObject.SetActive(true);
            RefsManager.I.DoppelCharacter.StartRecording();
            CamerasManager.I.ToggleSingleDoppelCams(true);

            animator.SetFloat("MoveSpeed", 0);
            ResetInputActions();
            rec = true;
            enabled = false;
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
        }
    }
}
//TENGO QUE GUARDAR EL INPUT EN TIEMPO REAL EN STATIC PARA PODER COMPARTIRLO BIEN ENTRE EL PERSONAJE Y EL DOPPEL.
//LOS BOOLEANOS DE MOVIMIENTO (FORWARD,LEFT,RIGHT,BACK) TIENEN QUE SER ESTÁTICOS PARA RELACIONAR PERSONAJE Y DOPPEL.