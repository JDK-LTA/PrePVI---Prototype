using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferencesManager : Singleton<ReferencesManager>
{
    [Header("Debug Variables")]
    [SerializeField] private bool DEBUG = false;

    [Header("Player references")]
    [SerializeField] private InputRecAndPlay inputRecAndPlay;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private DoppelMovement doppelMovement;
    [SerializeField] private PlayerCharacter playerCharacter;
    [SerializeField] private DoppelCharacter doppelCharacter;
   
    public bool _DEBUG { get => DEBUG; }
    public InputRecAndPlay InputRecAndPlay { get => inputRecAndPlay; }
    public PlayerMovement PlayerMovement { get => playerMovement; }
    public DoppelMovement DoppelMovement { get => doppelMovement; }
    public PlayerCharacter PlayerCharacter { get => playerCharacter; }
    public DoppelCharacter DoppelCharacter { get => doppelCharacter; }
}
