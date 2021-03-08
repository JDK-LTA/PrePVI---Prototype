using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoppelCharacter : BaseCharacter
{
    [Header("Debug Variables")]
    [SerializeField] private List<BinaryInputs> pressedBinInputs = new List<BinaryInputs>();
    [SerializeField] private List<BinaryInputs> releasedBinInputs = new List<BinaryInputs>();
    [SerializeField] private List<float> pressingTimes = new List<float>();
    [SerializeField] private List<float> releasingTimes = new List<float>();
    [SerializeField] private List<bool> playbackPressed = new List<bool>();
    [SerializeField] private List<bool> playbackReleased = new List<bool>();
    [SerializeField] private Queue<Vector2> continousInput = new Queue<Vector2>();

    [Header("Settings")]
    [SerializeField] private float timeRecording = 3f;
    private bool playback = false;

    private float t = 0;
    private int framesRecorded = 0, framesPlayed = 0;

    private Vector2 playXzInput = Vector2.zero;

    private bool isFree = false;

    public bool IsFree { get => isFree; }
    public float TimeRecording { get => timeRecording; set => timeRecording = value; }


    #region OVERRIDES
    protected override void Update()
    {
        if (rec)
        {
            RecordTimer();
            BinInputs();
        }
        if (playback)
        {
            PlaybackTimer();
        }
        isFree = playback;

        base.Update();
    }
    protected void FixedUpdate()
    {
        if (rec)
        {
            ContinousInput();
            RecordContInput();
            RefsManager.I.Vfx_ParticleChain.SetVector3(Shader.PropertyToID("DistanceToDoppel"),
                transform.position - RefsManager.I.PlayerCharacter.transform.position);
        }
        if (playback)
        {
            PlaybackContInput();
        }
    }
    protected override void XZMove()
    {
        if (rec)
            base.XZMove();
        else
        {
            base.XZMove(playXzInput);
        }
    }
    protected override void StartAttack()
    {
        for (int i = 0; i < RefsManager.I.Vfx_chargeAttack.Length; i++)
        {
            RefsManager.I.Vfx_chargeAttackDoppel[i].Play();

        }
    }
    protected override void MidAttack()
    {
        for (int i = 0; i < RefsManager.I.Vfx_projectile.Length; i++)
        {
            RefsManager.I.Vfx_projectileDoppel[i].Play();
        }
    }
    protected override void EndAttack()
    {
        for (int i = 0; i < RefsManager.I.Vfx_releaseAttack.Length; i++)
        {
            RefsManager.I.Vfx_releaseAttackDoppel[i].Play();
            RefsManager.I.Vfx_impactDoppel[i].Play();
        }
    }
    protected override void StartAttack2()
    {
        if (isFree)
        {
            RefsManager.I.Vfx_Attack2ForwardDoppel.SetTrigger("Trail");
            RefsManager.I.Vfx_Attack22ForwardDoppel.SetTrigger("Trail");
        }
    }
    #endregion
    #region REC AND PLAY
    public void StartRecording()
    {
        transform.position = RefsManager.I.PlayerCharacter.transform.position;
        transform.rotation = RefsManager.I.PlayerCharacter.transform.rotation;
        Physics.SyncTransforms();

        RefsManager.I.PlayerCharacter.enabled = false;

        ClearRecording();

        canStartRecording = false;
    }
    private void StartPlayback()
    {
        transform.position = RefsManager.I.PlayerCharacter.transform.position;
        transform.rotation = RefsManager.I.PlayerCharacter.transform.rotation;

        Physics.SyncTransforms();

        animator.SetTrigger("ResetAll");

        playback = true;
    }

    private void Recording()
    {
        //PRESSES
        if (Input.GetButtonDown("Jump") && canDoAnythingElse)
        {
            RecordKey(true, BinaryInputs.JUMP);
        }
        if (Input.GetButtonDown("Dash") && canDoAnythingElse)
        {
            RecordKey(true, BinaryInputs.DASH);
        }
        if (canAttack)
        {
            if (Input.GetButtonDown("Attack1") && canDoAnythingElse && velocity == Vector3.zero)
            {
                RecordKey(true, BinaryInputs.ATTACK1_STATIC);
            }
            else if (Input.GetButtonDown("Attack1") && canDoAnythingElse && velocity.magnitude > 0f)
            {
                RecordKey(true, BinaryInputs.ATTACK1_MOVE);
            }
            if (Input.GetButtonDown("Attack2") && canDoAnythingElse)
            {
                RecordKey(true, BinaryInputs.ATTACK2);
            }
        }
        //------------------------------------------------

        //RELEASES
        //if (Input.GetButtonUp("Jump"))
        //{
        //    RecordKey(false, BinaryInputs.JUMP);
        //}
        //if (Input.GetButtonUp("Dash"))
        //{
        //    RecordKey(false, BinaryInputs.DASH);
        //}
        //if (Input.GetButtonUp("Attack1"))
        //{
        //    RecordKey(false, BinaryInputs.ATTACK1);
        //}
        //if (Input.GetButtonUp("Attack2"))
        //{
        //    RecordKey(false, BinaryInputs.ATTACK2);
        //}
        //------------------------------------------------
    }
    private void RecordContInput()
    {
        continousInput.Enqueue(xzInput);
        framesRecorded++;
    }
    private void RecordKey(bool press, BinaryInputs inp)
    {
        if (press)
            pressedBinInputs.Add(inp);
        else
            releasedBinInputs.Add(inp);

        AddTimeAndBool(press);
    }

    private void Playback()
    {
        for (int i = 0; i < pressingTimes.Count; i++)
        {
            //Play Input presses
            if (t >= pressingTimes[i] && !playbackPressed[i])
            {
                playbackPressed[i] = true;

                switch (pressedBinInputs[i])
                {
                    case BinaryInputs.DASH:
                        Dash();
                        break;
                    case BinaryInputs.ATTACK1_STATIC:
                        Attack1();
                        break;
                    case BinaryInputs.ATTACK1_MOVE:
                        Attack1OnMove();
                        break;
                    case BinaryInputs.ATTACK2:
                        Attack2();
                        break;
                    case BinaryInputs.JUMP:
                        Jump();
                        break;
                }
            }
        }
    }
    private void PlaybackContInput()
    {
        if (framesPlayed < framesRecorded)
        {
            playXzInput = continousInput.Dequeue();
            framesPlayed++;
        }
    }

    private void FinishRecording()
    {
        t = 0;
        rec = false;

        ResetDashStuff();
        canDoAnythingElse = true;
        canAttack = true;
        RefsManager.I.PlayerCharacter.enabled = true;
        RefsManager.I.PlayerCharacter.Animator.enabled = true;
        StartPlayback();
        RefsManager.I.Vfx_ParticleChain.gameObject.SetActive(false);
    }

    private void FinishPlayback()
    {
        t = 0;
        canStartRecording = true;
        playback = false;
        canDoAnythingElse = true;
        canAttack = true;
        ResetDashStuff();

        gameObject.SetActive(false);
        CamerasManager.I.ToggleSingleDoppelCams(false);
        EnemyCactus cactus = FindObjectOfType<EnemyCactus>();
        if (cactus)
        {
            cactus.DoppelInRange = false;
            cactus.UpdateTarget();
        }
    }

    //AUXILIAR METHODS
    private void ClearRecording()
    {
        t = 0;
        pressedBinInputs.Clear();
        releasedBinInputs.Clear();
        pressingTimes.Clear();
        releasingTimes.Clear();
        playbackPressed.Clear();
        playbackReleased.Clear();
    }
    private void AddTimeAndBool(bool press)
    {
        float aux = t;
        if (press)
        {
            pressingTimes.Add(aux);
            playbackPressed.Add(false);
        }
        else
        {
            releasingTimes.Add(aux);
            playbackReleased.Add(false);
        }
    }
    private void ResetDashStuff()
    {
        if (!canDash)
        {
            canDash = true;
            dashReset = false;
            dashingNow = false;
            dashResetT = 0;
            dashT = 0;
        }
    }
    #endregion
    #region TIMERS
    private void RecordTimer()
    {
        t += Time.deltaTime;
        Recording();

        if (t >= timeRecording)
        {
            FinishRecording();
        }
    }
    private void PlaybackTimer()
    {
        t += Time.deltaTime;
        Playback();

        if (t >= timeRecording) //REMEMBER TO ADD VARIABLE FOR WHEN CUTTING THE DOPPEL SHORT / CANCELLING IT MID-ACTION
        {
            FinishPlayback();
        }
    }
    #endregion
}
