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

    private bool record = false, playback = false, canStartRecording = true, canPlayback = false;
    private float t = 0;
    [SerializeField] private float timeRecording = 3f;

    // Update is called once per frame
    private void Update()
    {
        if (canStartRecording && Input.GetKeyDown(keyToRecord))
        {
            ClearRecording();
            record = true;
            canStartRecording = false;
        }
        if (canPlayback && Input.GetKeyDown(keyToPlayback))
        {
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
        }
    }


    private void Recording() //RECORD PRESENT PRESSED INPUT AS INPUT PRESSED AT 0.0f
    {
        //PRESSES
        if (Input.GetKeyDown(keyToForward))
        {
            pressedKeyCodes.Add(GameInputs.FORWARD);
            AddTimeAndBool(true);
        }
        if (Input.GetKeyDown(keyToLeft))
        {
            pressedKeyCodes.Add(GameInputs.LEFT);
            AddTimeAndBool(true);
        }
        if (Input.GetKeyDown(keyToBack))
        {
            pressedKeyCodes.Add(GameInputs.BACK);
            AddTimeAndBool(true);
        }
        if (Input.GetKeyDown(keyToRight))
        {
            pressedKeyCodes.Add(GameInputs.RIGHT);
            AddTimeAndBool(true);
        }
        if (Input.GetKeyDown(keyToJump))
        {
            pressedKeyCodes.Add(GameInputs.JUMP);
            AddTimeAndBool(true);
        }
        //------------------------------------------------

        //RELEASES
        if (Input.GetKeyUp(keyToForward))
        {
            releasedKeyCodes.Add(GameInputs.FORWARD);
            AddTimeAndBool(false);
        }
        if (Input.GetKeyUp(keyToLeft))
        {
            releasedKeyCodes.Add(GameInputs.LEFT);
            AddTimeAndBool(false);
        }
        if (Input.GetKeyUp(keyToBack))
        {
            releasedKeyCodes.Add(GameInputs.BACK);
            AddTimeAndBool(false);
        }
        if (Input.GetKeyUp(keyToRight))
        {
            releasedKeyCodes.Add(GameInputs.RIGHT);
            AddTimeAndBool(false);
        }
        if (Input.GetKeyUp(keyToJump))
        {
            releasedKeyCodes.Add(GameInputs.JUMP);
            AddTimeAndBool(false);
        }
        //------------------------------------------------
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
        pressedKeyCodes.Clear();
        releasedKeyCodes.Clear();
        pressingTimes.Clear();
        releasingTimes.Clear();
        playbackPressed.Clear();
        playbackReleased.Clear();
    }


    private void Forward(bool pressed = true)
    {
        InputDebugMessage(pressed, "Forward");
    }
    private void Left(bool pressed = true)
    {
        InputDebugMessage(pressed, "Left");
    }
    private void Back(bool pressed = true)
    {
        InputDebugMessage(pressed, "Back");
    }
    private void Right(bool pressed = true)
    {
        InputDebugMessage(pressed, "Right");
    }
    private void Jump(bool pressed = true)
    {
        InputDebugMessage(pressed, "Jump");
    }

    private void InputDebugMessage(bool pressed, string name)
    {
        string aux = pressed ? "pressed" : "released";
        print(name + " " + aux + ": " + (float)Mathf.Round(t * 100f) / 100f);
    }
}
