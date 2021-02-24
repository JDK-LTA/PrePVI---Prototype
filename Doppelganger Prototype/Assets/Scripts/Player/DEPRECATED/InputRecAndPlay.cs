//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;



//public class InputRecAndPlay : MonoBehaviour
//{
//    [SerializeField] private List<BinaryInputs> pressedKeyCodes = new List<BinaryInputs>();
//    [SerializeField] private List<BinaryInputs> releasedKeyCodes = new List<BinaryInputs>();
//    [SerializeField] private List<float> pressingTimes = new List<float>();
//    [SerializeField] private List<float> releasingTimes = new List<float>();
//    [SerializeField] private List<bool> playbackPressed = new List<bool>();
//    [SerializeField] private List<bool> playbackReleased = new List<bool>();

//    [SerializeField] private KeyCode keyToRecord = KeyCode.R;
//    [SerializeField] private KeyCode keyToPlayback = KeyCode.P;
//    [SerializeField] private KeyCode keyToForward = KeyCode.W;
//    [SerializeField] private KeyCode keyToLeft = KeyCode.A;
//    [SerializeField] private KeyCode keyToBack = KeyCode.S;
//    [SerializeField] private KeyCode keyToRight = KeyCode.D;
//    [SerializeField] private KeyCode keyToJump = KeyCode.Space;

//    [SerializeField] private float timeRecording = 3f;
//    private bool record = false, playback = false, canStartRecording = true, canPlayback = false;
//    private float t = 0;

//    private bool[] wasdInput = new bool[4];

//    public delegate void InputBinaries(bool tr);
//    public event InputBinaries I_Forward, I_Back, I_Left, I_Right;
//    public event InputBinaries ID_Forward, ID_Back, ID_Left, ID_Right;
//    public delegate void InputTriggers();
//    public event InputTriggers I_Jump;
//    public event InputTriggers ID_Jump;
//    public event InputTriggers I_Reset;

//    // Update is called once per frame
//    private void Update()
//    {
//        if (canStartRecording && Input.GetKeyDown(keyToRecord))
//        {
//            StartRecording();
//        }
//        if (canPlayback && Input.GetKeyDown(keyToPlayback))
//        {
//            StartPlayback();
//        }


//        if (record)
//        {
//            RecordTimer();
//        }
//        if (playback)
//        {
//            PlaybackTimer();
//        }
//        Inputs(record);

//    }

//    private void StartRecording()
//    {
//        ReferencesManager.I.DoppelMovement.gameObject.SetActive(true);
//        ReferencesManager.I.DoppelMovement.transform.position = transform.position;
//        ReferencesManager.I.DoppelMovement.transform.rotation = transform.rotation;
//        Physics.SyncTransforms();
//        ReferencesManager.I.PlayerMovement.enabled = false;
//        ReferencesManager.I.DoppelMovement.DoppelSpecialSubscribe(false);
//        ReferencesManager.I.DoppelMovement.InitPlayPos = ReferencesManager.I.DoppelMovement.transform.position;
//        ReferencesManager.I.DoppelMovement.InitPlayRot = ReferencesManager.I.DoppelMovement.transform.eulerAngles;
        
//        ClearRecording();
//        InitialRecording();
        
//        record = true;
//        canStartRecording = false;
//    }

//    private void StartPlayback()
//    {
//        ReferencesManager.I.DoppelMovement.DoppelSpecialSubscribe(true);
//        ReferencesManager.I.DoppelMovement.transform.position = ReferencesManager.I.DoppelMovement.InitPlayPos;
//        ReferencesManager.I.DoppelMovement.transform.eulerAngles = ReferencesManager.I.DoppelMovement.InitPlayRot;
//        ReferencesManager.I.PlayerMovement.enabled = true;
//        Physics.SyncTransforms();
//        playback = true;
//        //canPlayback = false;
//    }

//    private void Inputs(bool recording = false)
//    {
//        //PRESSES
//        if (Input.GetKeyDown(keyToForward))
//        {
//            Forward();
//            if (!wasdInput[0])
//                wasdInput[0] = true;
//        }
//        if (Input.GetKeyDown(keyToLeft))
//        {
//            Left();
//            if (!wasdInput[1])
//                wasdInput[1] = true;
//        }
//        if (Input.GetKeyDown(keyToBack))
//        {
//            Back();
//            if (!wasdInput[2])
//                wasdInput[2] = true;
//        }
//        if (Input.GetKeyDown(keyToRight))
//        {
//            Right();
//            if (!wasdInput[3])
//                wasdInput[3] = true;
//        }
//        if (Input.GetKeyDown(keyToJump))
//        {
//            Jump();
//        }
//        //------------------------------------------------

//        //RELEASES
//        if (Input.GetKeyUp(keyToForward))
//        {
//            Forward(false);
//            if (wasdInput[0])
//                wasdInput[0] = false;
//        }
//        if (Input.GetKeyUp(keyToLeft))
//        {
//            Left(false);
//            if (wasdInput[1])
//                wasdInput[1] = false;
//        }
//        if (Input.GetKeyUp(keyToBack))
//        {
//            Back(false);
//            if (wasdInput[2])
//                wasdInput[2] = false;
//        }
//        if (Input.GetKeyUp(keyToRight))
//        {
//            Right(false);
//            if (wasdInput[3])
//                wasdInput[3] = false;
//        }
//        //------------------------------------------------
//    }

//    private void RecordTimer()
//    {
//        t += Time.deltaTime;
//        Recording();

//        if (t >= timeRecording)
//        {
//            t = 0;
//            record = false;
//            canPlayback = true;
//            StartPlayback();
//        }
//    }
//    private void PlaybackTimer()
//    {
//        t += Time.deltaTime;
//        Playback();

//        if (t >= timeRecording) //REMEMBER TO ADD VARIABLE FOR WHEN CUTTING THE DOPPEL SHORT / CANCELLING IT MID-ACTION
//        {
//            t = 0;
//            ReferencesManager.I.DoppelMovement.gameObject.SetActive(false);
//            canStartRecording = true;
//            playback = false;
//            I_Reset?.Invoke();
//        }
//    }


//    private void Recording() //RECORD PRESENT PRESSED INPUT AS INPUT PRESSED AT 0.0f
//    {
//        //PRESSES
//        if (Input.GetKeyDown(keyToForward))
//        {
//            RecordKey(true, BinaryInputs.FORWARD);
//        }
//        if (Input.GetKeyDown(keyToLeft))
//        {
//            RecordKey(true, BinaryInputs.LEFT);
//        }
//        if (Input.GetKeyDown(keyToBack))
//        {
//            RecordKey(true, BinaryInputs.BACK);
//        }
//        if (Input.GetKeyDown(keyToRight))
//        {
//            RecordKey(true, BinaryInputs.RIGHT);
//        }
//        if (Input.GetKeyDown(keyToJump))
//        {
//            RecordKey(true, BinaryInputs.JUMP);
//        }
//        //------------------------------------------------

//        //RELEASES
//        if (Input.GetKeyUp(keyToForward))
//        {
//            RecordKey(false, BinaryInputs.FORWARD);
//        }
//        if (Input.GetKeyUp(keyToLeft))
//        {
//            RecordKey(false, BinaryInputs.LEFT);
//        }
//        if (Input.GetKeyUp(keyToBack))
//        {
//            RecordKey(false, BinaryInputs.BACK);
//        }
//        if (Input.GetKeyUp(keyToRight))
//        {
//            RecordKey(false, BinaryInputs.RIGHT);
//        }
//        if (Input.GetKeyUp(keyToJump))
//        {
//            RecordKey(false, BinaryInputs.JUMP);
//        }
//        //------------------------------------------------
//    }
//    private void RecordKey(bool press, BinaryInputs inp)
//    {
//        if (press)
//            pressedKeyCodes.Add(inp);
//        else
//            releasedKeyCodes.Add(inp);

//        AddTimeAndBool(press);
//    }
//    private void Playback()
//    {
//        for (int i = 0; i < pressingTimes.Count; i++)
//        {
//            //Play Input presses
//            if (t >= pressingTimes[i] && !playbackPressed[i])
//            {
//                playbackPressed[i] = true;

//                switch (pressedKeyCodes[i])
//                {
//                    case BinaryInputs.FORWARD:
//                        Forward(true, true);
//                        break;
//                    case BinaryInputs.LEFT:
//                        Left(true, true);
//                        break;
//                    case BinaryInputs.BACK:
//                        Back(true, true);
//                        break;
//                    case BinaryInputs.RIGHT:
//                        Right(true, true);
//                        break;
//                    case BinaryInputs.JUMP:
//                        Jump(true, true);
//                        break;
//                }
//            }
//            //------------------------------------------------

//            //Play Input releases
//            if (i < releasingTimes.Count)
//            {
//                if (t >= releasingTimes[i] && !playbackReleased[i])
//                {
//                    playbackReleased[i] = true;

//                    switch (releasedKeyCodes[i])
//                    {
//                        case BinaryInputs.FORWARD:
//                            Forward(false, true);
//                            break;
//                        case BinaryInputs.LEFT:
//                            Left(false, true);
//                            break;
//                        case BinaryInputs.BACK:
//                            Back(false, true);
//                            break;
//                        case BinaryInputs.RIGHT:
//                            Right(false, true);
//                            break;
//                        case BinaryInputs.JUMP:
//                            Jump(false, true);
//                            break;
//                    }
//                }
//            }
//            //------------------------------------------------
//        }
//    }


//    private void AddTimeAndBool(bool press)
//    {
//        float aux = t;
//        if (press)
//        {
//            pressingTimes.Add(aux);
//            playbackPressed.Add(false);
//        }
//        else
//        {
//            releasingTimes.Add(aux);
//            playbackReleased.Add(false);
//        }
//    }
//    private void ClearRecording()
//    {
//        t = 0;
//        pressedKeyCodes.Clear();
//        releasedKeyCodes.Clear();
//        pressingTimes.Clear();
//        releasingTimes.Clear();
//        playbackPressed.Clear();
//        playbackReleased.Clear();
//    }
//    private void InitialRecording()
//    {
//        if (wasdInput[0])
//        {
//            RecordKey(true, BinaryInputs.FORWARD);
//            I_Forward(true);
//        }
//        if (wasdInput[1])
//        {
//            RecordKey(true, BinaryInputs.LEFT);
//            I_Left(true);
//        }
//        if (wasdInput[2])
//        {
//            RecordKey(true, BinaryInputs.BACK);
//            I_Back(true);
//        }
//        if (wasdInput[3])
//        {
//            RecordKey(true, BinaryInputs.RIGHT);
//            I_Right(true);
//        }
//    }

//    private void Forward(bool pressed = true, bool isPlaybacking = false)
//    {
//        InputDebugMessage(pressed, "Forward");

//        if (!isPlaybacking)
//            I_Forward?.Invoke(pressed);
//        else
//            ID_Forward?.Invoke(pressed);
//    }
//    private void Left(bool pressed = true, bool isPlaybacking = false)
//    {
//        InputDebugMessage(pressed, "Left");
//        if (!isPlaybacking)
//            I_Left?.Invoke(pressed);
//        else
//            ID_Left?.Invoke(pressed);

//    }
//    private void Back(bool pressed = true, bool isPlaybacking = false)
//    {
//        InputDebugMessage(pressed, "Back");
//        if (!isPlaybacking)
//            I_Back?.Invoke(pressed);
//        else
//            ID_Back?.Invoke(pressed);
//    }
//    private void Right(bool pressed = true, bool isPlaybacking = false)
//    {
//        InputDebugMessage(pressed, "Right");
//        if (!isPlaybacking)
//            I_Right?.Invoke(pressed);
//        else
//            ID_Right?.Invoke(pressed);
//    }
//    private void Jump(bool pressed = true, bool isPlaybacking = false)
//    {
//        InputDebugMessage(pressed, "Jump");
//        if (!isPlaybacking)
//        {
//            if (pressed)
//                I_Jump?.Invoke();
//        }
//        else
//        {
//            if (pressed)
//                ID_Jump?.Invoke();
//        }


//    }

//    private void InputDebugMessage(bool pressed, string name)
//    {
//        string aux = pressed ? "pressed" : "released";
//        if (ReferencesManager.I._DEBUG) print(name + " " + aux + ": " + (float)Mathf.Round(t * 100f) / 100f);
//    }
//}
