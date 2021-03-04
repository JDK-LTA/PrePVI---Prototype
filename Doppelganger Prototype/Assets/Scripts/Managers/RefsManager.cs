using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.VFX;

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
    

    [Header("VFX REFERENCES")]
    [SerializeField] private VisualEffect particleChainGO;
    [SerializeField] private Animator vfx_Attack1ForwardSimpleEffect;
    [SerializeField] private Animator vfx_Attack2ForwardSimpleEffect;
    [SerializeField] private Animator vfx_Attack22ForwardSimpleEffect;
    [SerializeField] private Animator vfx_Attack1ForwardDoppel;
    [SerializeField] private Animator vfx_Attack2ForwardDoppel;
    [SerializeField] private Animator vfx_Attack22ForwardDoppel;
    [SerializeField] private VisualEffect vfx_Attack1ParticlesUp;
    [SerializeField] private VisualEffect vfx_Attack2ParticlesUp;
    [SerializeField] private VisualEffect vfx_Attack22ParticlesUp;

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
    public VisualEffect ParticleChainGO { get => particleChainGO; set => particleChainGO = value; }
    public Animator Vfx_Attack1ForwardSimpleEffect { get => vfx_Attack1ForwardSimpleEffect; set => vfx_Attack1ForwardSimpleEffect = value; }
    public Animator Vfx_Attack2ForwardSimpleEffect { get => vfx_Attack2ForwardSimpleEffect; set => vfx_Attack2ForwardSimpleEffect = value; }
    public Animator Vfx_Attack22ForwardSimpleEffect { get => vfx_Attack22ForwardSimpleEffect; set => vfx_Attack22ForwardSimpleEffect = value; }
    public Animator Vfx_Attack1ForwardDoppel { get => vfx_Attack1ForwardDoppel; set => vfx_Attack1ForwardDoppel = value; }
    public Animator Vfx_Attack2ForwardDoppel { get => vfx_Attack2ForwardDoppel; set => vfx_Attack2ForwardDoppel = value; }
    public Animator Vfx_Attack22ForwardDoppel { get => vfx_Attack22ForwardDoppel; set => vfx_Attack22ForwardDoppel = value; }
    public VisualEffect Vfx_Attack1ParticlesUp { get => vfx_Attack1ParticlesUp; set => vfx_Attack1ParticlesUp = value; }
    public VisualEffect Vfx_Attack2ParticlesUp { get => vfx_Attack2ParticlesUp; set => vfx_Attack2ParticlesUp = value; }
    public VisualEffect Vfx_Attack22ParticlesUp { get => vfx_Attack22ParticlesUp; set => vfx_Attack22ParticlesUp = value; }
}
