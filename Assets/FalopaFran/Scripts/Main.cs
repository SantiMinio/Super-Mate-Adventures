using System;
using System.Collections;
using System.Collections.Generic;
using Frano;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main instance;
    
    [SerializeField] private CharacterHead _characterHead;


    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public CharacterHead GetMainCharacter => _characterHead;
}
