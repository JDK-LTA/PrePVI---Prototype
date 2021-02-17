using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferencesManager : Singleton<ReferencesManager>
{
    [Header("Debug Variables")]
    [SerializeField] private bool DEBUG = false;

    [Header("PLAYER REFERENCES")]
    [SerializeField] private PlayerCharacter playerCharacter;
    [SerializeField] private DoppelCharacter doppelCharacter;
    [Header("__deprecated__")]
    [SerializeField] private InputRecAndPlay inputRecAndPlay;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private DoppelMovement doppelMovement;
    //[Header("___________")]

    public bool _DEBUG { get => DEBUG; }
    public InputRecAndPlay InputRecAndPlay { get => inputRecAndPlay; }
    public PlayerMovement PlayerMovement { get => playerMovement; }
    public DoppelMovement DoppelMovement { get => doppelMovement; }
    public PlayerCharacter PlayerCharacter { get => playerCharacter; }
    public DoppelCharacter DoppelCharacter { get => doppelCharacter; }
}
