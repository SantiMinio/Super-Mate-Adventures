using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public int currentWaveNumber = 1;
    public int prevWaveNumber = 0;
    public int currentEnemiesInWave;
    [SerializeField] private int waveEnemyScaler = 2;
    [SerializeField] private int baseEnemiesPerWave;

    [SerializeField] private List<DummySpawner> spawners = new List<DummySpawner>();

    public bool waveActive;

    private void Start()
    {
        Main.instance.EventManager.SubscribeToEvent(GameEvent.SpawnCookie, OnSpawn);
        Main.instance.EventManager.SubscribeToEvent(GameEvent.TermoClicked, StartNewWave);
        Main.instance.EventManager.SubscribeToEvent(GameEvent.EnemyDead, IsWaveKilled);
        
        Main.instance.EventManager.SubscribeToEvent(GameEvent.MateDead, MateDead);
    }

    void MateDead()
    {
        foreach (var spawn in spawners)
        {
            foreach (var enemy in spawn.GetEnemiesSpawned())
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }
    
   

    private void IsWaveKilled()
    {
        if (!spawners.Any(x => { Debug.Log(x.GetLiveEnemies() + x.name); return x.GetLiveEnemies() > 0; }))
        {
            if (waveActive)
            {
                waveActive = false;
                Debug.Log("termino la wave");    
                Main.instance.EventManager.TriggerEvent(GameEvent.KilledAllEnemiesSpawned, currentEnemiesInWave);
            }
            
            //Debug.Log("termino la wave");  
        }
        
        // Debug.Log("chequeo si mataron enemigos");
        //
        // foreach (var spawner in spawners)
        // {
        //     //Debug.Log(spawner.AreEnemiesSpawnedAlive());
        //     if (spawner.AreEnemiesSpawnedAlive()) return;
        // }
        //
        // Main.instance.EventManager.TriggerEvent(GameEvent.KilledAllEnemiesSpawned);
        // Debug.Log("Se mataron todos los enemigos");
    }

    public bool SpawnersActive()
    { 
        return spawners.Any(x => x.active);
    }

    public void StartNewWave()
    {

        if(prevWaveNumber == currentWaveNumber) return;

        waveActive = true;
        
        prevWaveNumber = currentWaveNumber;
        Main.instance.EventManager.TriggerEvent(GameEvent.StartNewWave, currentWaveNumber);
        
        
    }
    
    
    
    private void OnSpawn()
    {
        currentEnemiesInWave++;

        if (currentEnemiesInWave >= Mathf.RoundToInt(currentWaveNumber * waveEnemyScaler) + baseEnemiesPerWave) FinishWave();
    }

    private void FinishWave()
    {
        currentEnemiesInWave = 0;
        currentWaveNumber++;
        Main.instance.EventManager.TriggerEvent(GameEvent.FinishWaveSpawn);
    }
}
