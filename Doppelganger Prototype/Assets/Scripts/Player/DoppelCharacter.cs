using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoppelCharacter : BaseCharacter
{
    [Header("Debug Variables")]
    [SerializeField] private List<GameInputs> pressedKeyCodes = new List<GameInputs>();
    [SerializeField] private List<GameInputs> releasedKeyCodes = new List<GameInputs>();
    [SerializeField] private List<float> pressingTimes = new List<float>();
    [SerializeField] private List<float> releasingTimes = new List<float>();
    [SerializeField] private List<bool> playbackPressed = new List<bool>();
    [SerializeField] private List<bool> playbackReleased = new List<bool>();

    [Header("Settings")]
    [SerializeField] private float timeRecording = 3f;
    private bool playback = false;

    private float t = 0;

    protected override void Update()
    {
        if (rec)
        {
            Inputs();
            RecordTimer();
        }
        if (playback)
        {
            PlaybackTimer();
        }

        base.Update();
    }
    public void StartRecording()
    {
        transform.position = ReferencesManager.I.PlayerCharacter.transform.position;
        transform.rotation = ReferencesManager.I.PlayerCharacter.transform.rotation;
        Physics.SyncTransforms();

        ReferencesManager.I.PlayerCharacter.enabled = false;

        ClearRecording();
        InitialRecording();

        canStartRecording = false;
    }

    private void StartPlayback()
    {
        transform.position = ReferencesManager.I.PlayerCharacter.transform.position;
        transform.rotation = ReferencesManager.I.PlayerCharacter.transform.rotation;
        ReferencesManager.I.PlayerCharacter.ResetAndReinput();
        ResetInputActions();
        Physics.SyncTransforms();

        playback = true;
    }
    private void RecordTimer()
    {
        t += Time.deltaTime;
        Recording();

        if (t >= timeRecording)
        {
            t = 0;
            rec = false;
            ReferencesManager.I.PlayerCharacter.enabled = true;
            StartPlayback();
        }
    }
    private void PlaybackTimer()
    {
        t += Time.deltaTime;
        Playback();

        if (t >= timeRecording) //REMEMBER TO ADD VARIABLE FOR WHEN CUTTING THE DOPPEL SHORT / CANCELLING IT MID-ACTION
        {
            t = 0;
            canStartRecording = true;
            playback = false;
            gameObject.SetActive(false);
            ReferencesManager.I.PlayerCharacter.enabled = true;
        }
    }
    private void Recording()
    {
        //PRESSES
        if (Input.GetKeyDown(keyToForward))
        {
            RecordKey(true, GameInputs.FORWARD);
        }
        if (Input.GetKeyDown(keyToLeft))
        {
            RecordKey(true, GameInputs.LEFT);
        }
        if (Input.GetKeyDown(keyToBack))
        {
            RecordKey(true, GameInputs.BACK);
        }
        if (Input.GetKeyDown(keyToRight))
        {
            RecordKey(true, GameInputs.RIGHT);
        }
        if (Input.GetKeyDown(keyToJump))
        {
            RecordKey(true, GameInputs.JUMP);
        }
        //------------------------------------------------

        //RELEASES
        if (Input.GetKeyUp(keyToForward))
        {
            RecordKey(false, GameInputs.FORWARD);
        }
        if (Input.GetKeyUp(keyToLeft))
        {
            RecordKey(false, GameInputs.LEFT);
        }
        if (Input.GetKeyUp(keyToBack))
        {
            RecordKey(false, GameInputs.BACK);
        }
        if (Input.GetKeyUp(keyToRight))
        {
            RecordKey(false, GameInputs.RIGHT);
        }
        if (Input.GetKeyUp(keyToJump))
        {
            RecordKey(false, GameInputs.JUMP);
        }
        //------------------------------------------------
    }
    private void RecordKey(bool press, GameInputs inp)
    {
        if (press)
            pressedKeyCodes.Add(inp);
        else
            releasedKeyCodes.Add(inp);

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

                switch (pressedKeyCodes[i])
                {
                    case GameInputs.FORWARD:
                        forward = true;
                        break;
                    case GameInputs.LEFT:
                        left = true;
                        break;
                    case GameInputs.BACK:
                        back = true;
                        break;
                    case GameInputs.RIGHT:
                        right = true;
                        break;
                    case GameInputs.JUMP:
                        Jump();
                        break;
                }
            }
            //------------------------------------------------

            //Play Input releases
            if (i < releasingTimes.Count)
            {
                if (t >= releasingTimes[i] && !playbackReleased[i])
                {
                    playbackReleased[i] = true;

                    switch (releasedKeyCodes[i])
                    {
                        case GameInputs.FORWARD:
                            forward = false;
                            break;
                        case GameInputs.LEFT:
                            left = false;
                            break;
                        case GameInputs.BACK:
                            back = false;
                            break;
                        case GameInputs.RIGHT:
                            right = false;
                            break;
                        case GameInputs.JUMP:
                            break;
                    }
                }
            }
            //------------------------------------------------
        }
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
    private void ClearRecording()
    {
        t = 0;
        pressedKeyCodes.Clear();
        releasedKeyCodes.Clear();
        pressingTimes.Clear();
        releasingTimes.Clear();
        playbackPressed.Clear();
        playbackReleased.Clear();
    }
    private void InitialRecording()
    {
        ResetAndReinput();
        if (wasdInput[0])
        {
            RecordKey(true, GameInputs.FORWARD);
        }
        if (wasdInput[1])
        {
            RecordKey(true, GameInputs.LEFT);
        }
        if (wasdInput[2])
        {
            RecordKey(true, GameInputs.BACK);
        }
        if (wasdInput[3])
        {
            RecordKey(true, GameInputs.RIGHT);
        }
    }
}
