using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum GameInputs { FORWARD, LEFT, BACK, RIGHT, JUMP }

public class InputRecAndPlay : MonoBehaviour
{
    [SerializeField] private List<GameInputs> pressedKeyCodes = new List<GameInputs>();
    [SerializeField] private List<GameInputs> releasedKeyCodes = new List<GameInputs>();
    [SerializeField] private List<float> pressingTimes = new List<float>();
    [SerializeField] private List<float> releasingTimes = new List<float>();
    [SerializeField] private List<bool> playbackPressed = new List<bool>();
    [SerializeField] private List<bool> playbackReleased = new List<bool>();

    [SerializeField] private KeyCode keyToRecord = KeyCode.R;
    [SerializeField] private KeyCode keyToPlayback = KeyCode.P;
    [SerializeField] private KeyCode keyToForward = KeyCode.W;
    [SerializeField] private KeyCode keyToLeft = KeyCode.A;
    [SerializeField] private KeyCode keyToBack = KeyCode.S;
    [SerializeField] private KeyCode keyToRight = KeyCode.D;
    [SerializeField] private KeyCode keyToJump = KeyCode.Space;

    [SerializeField] private float timeRecording = 3f;
    private bool record = false, playback = false, canStartRecording = true, canPlayback = false;
    private float t = 0;

    private bool[] wasdInput = new bool[4];

    public Vector3 originalPos = Vector3.up;

    public delegate void InputBinaries(bool tr);
    public event InputBinaries I_Forward, I_Back, I_Left, I_Right;
    public delegate void InputTriggers();
    public event InputTriggers I_Jump;
    public event InputTriggers I_Reset;

    // Update is called once per frame
    private void Update()
    {
        if (canStartRecording && Input.GetKeyDown(keyToRecord))
        {
            ClearRecording();
            InitialRecording();
            originalPos = transform.position;
            record = true;
            canStartRecording = false;
        }
        if (canPlayback && Input.GetKeyDown(keyToPlayback))
        {
            transform.position = originalPos;
            Physics.SyncTransforms();
            playback = true;
            canPlayback = false;
        }


        if (record)
        {
            RecordTimer();
        }
        if (playback)
        {
            PlaybackTimer();
        }
        else
        {
            Inputs();
        }
    }

    private void Inputs()
    {
        //PRESSES
        if (Input.GetKeyDown(keyToForward))
        {
            Forward();
            if (!wasdInput[0])
                wasdInput[0] = true;
        }
        if (Input.GetKeyDown(keyToLeft))
        {
            Left();
            if (!wasdInput[1])
                wasdInput[1] = true;
        }
        if (Input.GetKeyDown(keyToBack))
        {
            Back();
            if (!wasdInput[2])
                wasdInput[2] = true;
        }
        if (Input.GetKeyDown(keyToRight))
        {
            Right();
            if (!wasdInput[3])
                wasdInput[3] = true;
        }
        if (Input.GetKeyDown(keyToJump))
        {
            Jump();
        }
        //------------------------------------------------

        //RELEASES
        if (Input.GetKeyUp(keyToForward))
        {
            Forward(false);
            if (wasdInput[0])
                wasdInput[0] = false;
        }
        if (Input.GetKeyUp(keyToLeft))
        {
            Left(false);
            if (wasdInput[1])
                wasdInput[1] = false;
        }
        if (Input.GetKeyUp(keyToBack))
        {
            Back(false);
            if (wasdInput[2])
                wasdInput[2] = false;
        }
        if (Input.GetKeyUp(keyToRight))
        {
            Right(false);
            if (wasdInput[3])
                wasdInput[3] = false;
        }
        //if (Input.GetKeyUp(keyToJump))
        //{
        //    releasedKeyCodes.Add(GameInputs.JUMP);
        //    AddTimeAndBool(false);
        //}
        //------------------------------------------------
    }

    private void RecordTimer()
    {
        t += Time.deltaTime;
        Recording();

        if (t >= timeRecording)
        {
            t = 0;
            record = false;
            canPlayback = true;
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
            I_Reset?.Invoke();
        }
    }


    private void Recording() //RECORD PRESENT PRESSED INPUT AS INPUT PRESSED AT 0.0f
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
                        Forward();
                        break;
                    case GameInputs.LEFT:
                        Left();
                        break;
                    case GameInputs.BACK:
                        Back();
                        break;
                    case GameInputs.RIGHT:
                        Right();
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
                            Forward(false);
                            break;
                        case GameInputs.LEFT:
                            Left(false);
                            break;
                        case GameInputs.BACK:
                            Back(false);
                            break;
                        case GameInputs.RIGHT:
                            Right(false);
                            break;
                        case GameInputs.JUMP:
                            Jump(false);
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
        if (wasdInput[0])
            RecordKey(true, GameInputs.FORWARD);
        if (wasdInput[1])
            RecordKey(true, GameInputs.LEFT);
        if (wasdInput[2])
            RecordKey(true, GameInputs.BACK);
        if (wasdInput[3])
            RecordKey(true, GameInputs.RIGHT);
    }

    private void Forward(bool pressed = true)
    {
        InputDebugMessage(pressed, "Forward");
        I_Forward?.Invoke(pressed);
    }
    private void Left(bool pressed = true)
    {
        InputDebugMessage(pressed, "Left");
        I_Left?.Invoke(pressed);
    }
    private void Back(bool pressed = true)
    {
        InputDebugMessage(pressed, "Back");
        I_Back?.Invoke(pressed);
    }
    private void Right(bool pressed = true)
    {
        InputDebugMessage(pressed, "Right");
        I_Right?.Invoke(pressed);
    }
    private void Jump(bool pressed = true)
    {
        InputDebugMessage(pressed, "Jump");
        if (pressed)
            I_Jump?.Invoke();
    }

    private void InputDebugMessage(bool pressed, string name)
    {
        string aux = pressed ? "pressed" : "released";
        if (ReferencesManager.Instance._DEBUG) print(name + " " + aux + ": " + (float)Mathf.Round(t * 100f) / 100f);
    }
}
