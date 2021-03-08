using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.VFX;
using UnityEngine.UI;

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
    [SerializeField] private VisualEffect vfx_ParticleChain;
    [SerializeField] private Animator vfx_Attack2ForwardSimpleEffect;
    [SerializeField] private Animator vfx_Attack22ForwardSimpleEffect;
    [SerializeField] private Animator vfx_Attack2ForwardDoppel;
    [SerializeField] private Animator vfx_Attack22ForwardDoppel;
    [SerializeField] private VisualEffect vfx_Attack1ParticlesUp;
    [SerializeField] private VisualEffect vfx_Attack2ParticlesUp, vfx_Attack2ParticlesUpDoppel;
    [SerializeField] private VisualEffect vfx_Attack22ParticlesUp, vfx_Attack22ParticlesUpDoppel;
    [SerializeField] private VisualEffect[] vfx_HoloParticles;
    [SerializeField] private ParticleSystem[] vfx_chargeAttack, vfx_chargeAttackDoppel;
    [SerializeField] private ParticleSystem[] vfx_projectile, vfx_projectileDoppel;
    [SerializeField] private ParticleSystem[] vfx_releaseAttack, vfx_releaseAttackDoppel;
    [SerializeField] private ParticleSystem[] vfx_impact, vfx_impactDoppel;
    [SerializeField] private VisualEffect vfx_enemyCactusDeath;
    [SerializeField] private VisualEffect vfx_enemyCarnivoreDeath;
    [SerializeField] private VisualEffect vfx_playerDeath;


    [Header("UI REFERENCES")]
    [SerializeField] private Image player_LifeBar;
    [SerializeField] private GameObject pauseMenu, mainMenu;

    [Header("CAMERA REFERENCES")]
    [SerializeField] private List<SingleDoppelTuplas> singleDoppelTuplas;
    [SerializeField] private List<CinemachineVirtualCamera> levelVirtualCameras = new List<CinemachineVirtualCamera>();

    private CinemachineVirtualCamera activeCamera;
    private CinemachineVirtualCamera followActiveCamera;
    private CinemachineVirtualCamera actualSingleFollowCamera;
    private CinemachineVirtualCamera actualDoppelFollowCamera;

    public bool _DEBUG { get => DEBUG; }

    public PlayerCharacter PlayerCharacter { get => playerCharacter; }
    public DoppelCharacter DoppelCharacter { get => doppelCharacter; }
    public List<CinemachineVirtualCamera> LevelVirtualCameras { get => levelVirtualCameras; set => levelVirtualCameras = value; }
    public CinemachineVirtualCamera ActualSingleFollowCamera { get => actualSingleFollowCamera; set => actualSingleFollowCamera = value; }
    public CinemachineVirtualCamera ActualDoppelFollowCamera { get => actualDoppelFollowCamera; set => actualDoppelFollowCamera = value; }
    public CinemachineVirtualCamera ActiveCamera { get => activeCamera; set => activeCamera = value; }
    public CinemachineVirtualCamera FollowActiveCamera { get => followActiveCamera; set => followActiveCamera = value; }
    public List<SingleDoppelTuplas> SingleDoppelTuplas { get => singleDoppelTuplas; set => singleDoppelTuplas = value; }
    public VisualEffect Vfx_ParticleChain { get => vfx_ParticleChain; set => vfx_ParticleChain = value; }
    public Animator Vfx_Attack2ForwardSimpleEffect { get => vfx_Attack2ForwardSimpleEffect; set => vfx_Attack2ForwardSimpleEffect = value; }
    public Animator Vfx_Attack22ForwardSimpleEffect { get => vfx_Attack22ForwardSimpleEffect; set => vfx_Attack22ForwardSimpleEffect = value; }
    public Animator Vfx_Attack2ForwardDoppel { get => vfx_Attack2ForwardDoppel; set => vfx_Attack2ForwardDoppel = value; }
    public Animator Vfx_Attack22ForwardDoppel { get => vfx_Attack22ForwardDoppel; set => vfx_Attack22ForwardDoppel = value; }
    public VisualEffect Vfx_Attack1ParticlesUp { get => vfx_Attack1ParticlesUp; set => vfx_Attack1ParticlesUp = value; }
    public VisualEffect Vfx_Attack2ParticlesUp { get => vfx_Attack2ParticlesUp; set => vfx_Attack2ParticlesUp = value; }
    public VisualEffect Vfx_Attack22ParticlesUp { get => vfx_Attack22ParticlesUp; set => vfx_Attack22ParticlesUp = value; }
    public VisualEffect[] Vfx_HoloParticles { get => vfx_HoloParticles; set => vfx_HoloParticles = value; }
    public Image Player_LifeBar { get => player_LifeBar; set => player_LifeBar = value; }
    public ParticleSystem[] Vfx_chargeAttack { get => vfx_chargeAttack; set => vfx_chargeAttack = value; }
    public ParticleSystem[] Vfx_projectile { get => vfx_projectile; set => vfx_projectile = value; }
    public ParticleSystem[] Vfx_releaseAttack { get => vfx_releaseAttack; set => vfx_releaseAttack = value; }
    public ParticleSystem[] Vfx_impact { get => vfx_impact; set => vfx_impact = value; }
    public ParticleSystem[] Vfx_chargeAttackDoppel { get => vfx_chargeAttackDoppel; set => vfx_chargeAttackDoppel = value; }
    public ParticleSystem[] Vfx_projectileDoppel { get => vfx_projectileDoppel; set => vfx_projectileDoppel = value; }
    public ParticleSystem[] Vfx_releaseAttackDoppel { get => vfx_releaseAttackDoppel; set => vfx_releaseAttackDoppel = value; }
    public ParticleSystem[] Vfx_impactDoppel { get => vfx_impactDoppel; set => vfx_impactDoppel = value; }
    public GameObject PauseMenu { get => pauseMenu; }
    public GameObject MainMenu { get => mainMenu; }
    public VisualEffect Vfx_enemyCactusDeath { get => vfx_enemyCactusDeath; set => vfx_enemyCactusDeath = value; }
    public VisualEffect Vfx_enemyCarnivoreDeath { get => vfx_enemyCarnivoreDeath; set => vfx_enemyCarnivoreDeath = value; }
    public VisualEffect Vfx_playerDeath { get => vfx_playerDeath; set => vfx_playerDeath = value; }
    public VisualEffect Vfx_Attack2ParticlesUpDoppel { get => vfx_Attack2ParticlesUpDoppel; set => vfx_Attack2ParticlesUpDoppel = value; }
    public VisualEffect Vfx_Attack22ParticlesUpDoppel { get => vfx_Attack22ParticlesUpDoppel; set => vfx_Attack22ParticlesUpDoppel = value; }
}
