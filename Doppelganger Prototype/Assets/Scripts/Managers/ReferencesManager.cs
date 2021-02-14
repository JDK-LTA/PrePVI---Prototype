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
   
    public bool _DEBUG { get => DEBUG; }
    public InputRecAndPlay InputRecAndPlay { get => inputRecAndPlay; }
    public PlayerMovement PlayerMovement { get => playerMovement; set => playerMovement = value; }
}
