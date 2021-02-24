using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamerasManager : Singleton<CamerasManager>
{
    public bool isLevelCamActive = false;
    public bool isDoppel = false;

    private void Start()
    {
        RefsManager.I.ActualSingleFollowCamera = RefsManager.I.SingleDoppelTuplas[0].singleFollowCam;
        RefsManager.I.ActualDoppelFollowCamera = RefsManager.I.SingleDoppelTuplas[0].doppelFollowCam;
        RefsManager.I.ActiveCamera = RefsManager.I.ActualSingleFollowCamera;
        RefsManager.I.FollowActiveCamera = RefsManager.I.ActualSingleFollowCamera;
    }
    public void ChangeActiveCamera(CinemachineVirtualCamera newActiveCam)
    {
        RefsManager.I.ActiveCamera.enabled = false;
        newActiveCam.enabled = true;
        RefsManager.I.ActiveCamera = newActiveCam;
    }
    public void ChangeActiveCamera(int vcCamIndex)
    {
        RefsManager.I.ActiveCamera.enabled = false;
        RefsManager.I.LevelVirtualCameras[vcCamIndex].enabled = true;
        RefsManager.I.ActiveCamera = RefsManager.I.LevelVirtualCameras[vcCamIndex];
    }
    public void ToggleSingleDoppelCams(bool doppel)
    {
        if (!isLevelCamActive)
        {
            CinemachineVirtualCamera vcCam = doppel ? RefsManager.I.ActualDoppelFollowCamera : RefsManager.I.ActualSingleFollowCamera;
            ChangeActiveCamera(vcCam);
        }
        FollowCamToGoBackTo(doppel);
        isDoppel = doppel;
    }
    private void FollowCamToGoBackTo(bool doppel)
    {
        RefsManager.I.FollowActiveCamera = doppel ? RefsManager.I.ActualDoppelFollowCamera : RefsManager.I.ActualSingleFollowCamera;
    }
    public void ChangeCamTupla(int index)
    {
        RefsManager.I.ActualSingleFollowCamera = RefsManager.I.SingleDoppelTuplas[index].singleFollowCam; 
        RefsManager.I.ActualDoppelFollowCamera = RefsManager.I.SingleDoppelTuplas[index].doppelFollowCam;
        ToggleSingleDoppelCams(isDoppel);
    }
}
