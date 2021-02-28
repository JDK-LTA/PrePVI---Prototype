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

    #region OVERRIDES
    protected override void Update()
    {
        if (rec)
        {
            BinInputs();
            RecordTimer();
        }
        if (playback)
        {
            PlaybackTimer();
        }

        base.Update();
    }
    protected void FixedUpdate()
    {
        if (rec)
        {
            ContinousInput();
            RecordContInput();
            RefsManager.I.ParticleChainGO.SetVector3(Shader.PropertyToID("DistanceToDoppel"),
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
        if (Input.GetButtonDown("Attack1") && canDoAnythingElse)
        {
            RecordKey(true, BinaryInputs.ATTACK1);
        }
        if (Input.GetButtonDown("Attack2") && canDoAnythingElse)
        {
            RecordKey(true, BinaryInputs.ATTACK2);
        }       //------------------------------------------------

        //RELEASES
        if (Input.GetButtonUp("Jump"))
        {
            RecordKey(false, BinaryInputs.JUMP);
        }
        if (Input.GetButtonUp("Dash"))
        {
            RecordKey(false, BinaryInputs.DASH);
        }
        if (Input.GetButtonUp("Attack1"))
        {
            RecordKey(false, BinaryInputs.ATTACK1);
        }
        if (Input.GetButtonUp("Attack2"))
        {
            RecordKey(false, BinaryInputs.ATTACK2);
        }
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
                    case BinaryInputs.ATTACK1:
                        Attack1();
                        break;
                    case BinaryInputs.ATTACK2:
                        print("att2");
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
        RefsManager.I.PlayerCharacter.enabled = true;
        StartPlayback();
        RefsManager.I.ParticleChainGO.gameObject.SetActive(false);
    }
    private void FinishPlayback()
    {
        t = 0;
        canStartRecording = true;
        playback = false;
        gameObject.SetActive(false);
        CamerasManager.I.ToggleSingleDoppelCams(false);
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
