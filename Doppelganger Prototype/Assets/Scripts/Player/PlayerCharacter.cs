using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : BaseCharacter
{
    protected override void Update()
    {
        if (canStartRecording && Input.GetKeyDown(keyToRecord))
        {
            ReferencesManager.I.DoppelCharacter.gameObject.SetActive(true);
            ReferencesManager.I.DoppelCharacter.StartRecording();
            rec = true;
            enabled = false;
        }

        if (!rec)
            Inputs();

        base.Update();
    }
}
//TENGO QUE GUARDAR EL INPUT EN TIEMPO REAL EN STATIC PARA PODER COMPARTIRLO BIEN ENTRE EL PERSONAJE Y EL DOPPEL.
//LOS BOOLEANOS DE MOVIMIENTO (FORWARD,LEFT,RIGHT,BACK) TIENEN QUE SER ESTÁTICOS PARA RELACIONAR PERSONAJE Y DOPPEL.