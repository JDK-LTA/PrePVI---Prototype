using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferencesManager : Singleton<ReferencesManager>
{
    [SerializeField] private InputRecAndPlay inputRecAndPlay;
   
    public InputRecAndPlay InputRecAndPlay { get => inputRecAndPlay; }
}
