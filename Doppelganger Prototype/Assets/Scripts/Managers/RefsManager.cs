using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public struct SingleDoppelTuplas
{
    public CinemachineVirtualCamera singleFollowCam;
    public CinemachineVirtualCamera doppelFollowCam;
}

public class RefsManager : Singleton<RefsManager>
{
    [Header("Debug Variables")]
    [SerializeField] private bool DEBUG = false;

    [Header("PLAYER REFERENCES")]
    [SerializeField] private PlayerCharacter playerCharacter;
    [SerializeField] private DoppelCharacter doppelCharacter;
    [Header("CAMERA REFERENCES")]
    [SerializeField] private List<SingleDoppelTuplas> singleDoppelTuplas;
    [SerializeField] private List<CinemachineVirtualCamera> levelVirtualCameras = new List<CinemachineVirtualCamera>();
    private CinemachineVirtualCamera activeCamera;
    private CinemachineVirtualCamera followActiveCamera;
    private CinemachineVirtualCamera actualSingleFollowCamera;
    private CinemachineVirtualCamera actualDoppelFollowCamera;
    [Header("PUZZLE REFERENCES")]
    [SerializeField] private StepOnButton buttonSystem;

    //[Header("___________")]

    public bool _DEBUG { get => DEBUG; }

    public PlayerCharacter PlayerCharacter { get => playerCharacter; }
    public DoppelCharacter DoppelCharacter { get => doppelCharacter; }
    public StepOnButton ButtonSystem { get => buttonSystem; }
    public List<CinemachineVirtualCamera> LevelVirtualCameras { get => levelVirtualCameras; set => levelVirtualCameras = value; }
    public CinemachineVirtualCamera ActualSingleFollowCamera { get => actualSingleFollowCamera; set => actualSingleFollowCamera = value; }
    public CinemachineVirtualCamera ActualDoppelFollowCamera { get => actualDoppelFollowCamera; set => actualDoppelFollowCamera = value; }
    public CinemachineVirtualCamera ActiveCamera { get => activeCamera; set => activeCamera = value; }
    public CinemachineVirtualCamera FollowActiveCamera { get => followActiveCamera; set => followActiveCamera = value; }
    public List<SingleDoppelTuplas> SingleDoppelTuplas { get => singleDoppelTuplas; set => singleDoppelTuplas = value; }

}
