using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoppelMovement : BaseMovement
{
    private Vector3 initPlayPos = Vector3.zero, initPlayRot = Vector3.zero;

    public Vector3 InitPlayPos { get => initPlayPos; set => initPlayPos = value; }
    public Vector3 InitPlayRot { get => initPlayRot; set => initPlayRot = value; }

    public void DoppelSpecialSubscribe(bool subbing)
    {
        MainInputSubscribing(!subbing);
        //SpecialSubscribe(subbing);

        //if (subbing)
        //    transform.parent = null;
        //else
        //    transform.parent = ReferencesManager.I.PlayerMovement.transform;
    }

    //private void SpecialSubscribe(bool subbing)
    //{
    //    if (subbing)
    //    {
    //        ReferencesManager.I.InputRecAndPlay.ID_Forward += SetForward;
    //        ReferencesManager.I.InputRecAndPlay.ID_Back += SetBack;
    //        ReferencesManager.I.InputRecAndPlay.ID_Left += SetLeft;
    //        ReferencesManager.I.InputRecAndPlay.ID_Right += SetRight;
    //        ReferencesManager.I.InputRecAndPlay.ID_Jump += Jump;
    //        ReferencesManager.I.InputRecAndPlay.I_Reset += ResetInputActions;
    //    }
    //    else
    //    {
    //        ReferencesManager.I.InputRecAndPlay.ID_Forward -= SetForward;
    //        ReferencesManager.I.InputRecAndPlay.ID_Back -= SetBack;
    //        ReferencesManager.I.InputRecAndPlay.ID_Left -= SetLeft;
    //        ReferencesManager.I.InputRecAndPlay.ID_Right -= SetRight;
    //        ReferencesManager.I.InputRecAndPlay.ID_Jump -= Jump;
    //        ReferencesManager.I.InputRecAndPlay.I_Reset -= ResetInputActions;
    //    }
    //}

}
