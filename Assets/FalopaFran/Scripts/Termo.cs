using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Termo : MonoBehaviour
{
    [SerializeField] private LayerMask targets;
    public Animator anim;
    [SerializeField] private WaveManager _waveManager;

    private Collider myCol;
    
    private void Awake()
    {
        myCol = GetComponent<Collider>();
    }

    private void Start()
    {
        Main.instance.EventManager.SubscribeToEvent(GameEvent.StartNewWave,StartWave);
        Main.instance.EventManager.SubscribeToEvent(GameEvent.KilledAllEnemiesSpawned,FinishWave);
    }

    void StartWave()
    {
        myCol.enabled = false;
        anim.SetBool("On", false);
    }
    
    void FinishWave()
    {
        Debug.Log("sucede");
        anim.SetBool("On", true);
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
