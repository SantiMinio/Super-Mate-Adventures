using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Termo : MonoBehaviour
{
    [SerializeField] private LayerMask targets;

    [SerializeField] private WaveManager _waveManager;


    private void Start()
    {
        Main.instance.EventManager.SubscribeToEvent(GameEvent.StartNewWave, () => GetComponent<Collider>().enabled = false);
        Main.instance.EventManager.SubscribeToEvent(GameEvent.KilledAllEnemiesSpawned, () => GetComponent<Collider>().enabled = true);
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
