using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Termo : MonoBehaviour
{
    [SerializeField] private LayerMask targets;

    [SerializeField] private WaveManager _waveManager;

    private Collider myCol;
    
    private void Awake()
    {
        myCol = GetComponent<Collider>();
    }

    private void Start()
    {
        Main.instance.EventManager.SubscribeToEvent(GameEvent.StartNewWave,OffCollider);
        Main.instance.EventManager.SubscribeToEvent(GameEvent.KilledAllEnemiesSpawned,OnCollider);
    }

    void OffCollider()
    {
        myCol.enabled = false;
    }
    
    void OnCollider()
    {
        Debug.Log("sucede");
        myCol.enabled = true;
    }
    
    private void OnMouseDown()
    {
        if (_waveManager.SpawnersActive()) return;
      
        Main.instance.EventManager.TriggerEvent(GameEvent.TermoClicked);
        
        var sphereCheck = Physics.CheckSphere(transform.position, 15f, targets);

        if (sphereCheck)
        {
            Main.instance.GetMainCharacter.manaSystem.FillFullMana();
            Main.instance.GetMainCharacter.GetLifeHandler.ResetLife();
        }
    }
}
