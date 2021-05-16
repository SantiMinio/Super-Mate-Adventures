using System;
using System.Collections;
using System.Collections.Generic;
using Frano;
using FranoW.DevelopTools;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main instance;
    
    [SerializeField] private CharacterHead _characterHead;

    [SerializeField] private PlayableGrid _playableGrid;
    
    private EventManager _eventManager = new EventManager();

    public EventManager EventManager => _eventManager;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public CharacterHead GetMainCharacter => _characterHead;
    public PlayableGrid GetPlayableGrid => _playableGrid;
}
