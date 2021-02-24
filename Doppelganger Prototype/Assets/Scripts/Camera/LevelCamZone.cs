using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public enum TypeOfZone { STATIC, FOLLOW }

[RequireComponent(typeof(Collider))]
public class LevelCamZone : MonoBehaviour
{
    [SerializeField] private int virtualCameraIndex;
    [SerializeField] private TypeOfZone type;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerCharacter>())
        {
            if (type == TypeOfZone.STATIC)
            {
                CamerasManager.I.IsLevelCamActive = true;
                CamerasManager.I.ChangeActiveCamera(virtualCameraIndex);
            }
            else
            {
                CamerasManager.I.ChangeCamTupla(virtualCameraIndex);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerCharacter>())
        {
            if (type == TypeOfZone.STATIC)
            {
                CamerasManager.I.IsLevelCamActive = false;
                CamerasManager.I.ChangeActiveCamera(RefsManager.I.FollowActiveCamera);
            }
            else
            {
                CamerasManager.I.ChangeCamTupla(0);
            }
        }
    }
}
