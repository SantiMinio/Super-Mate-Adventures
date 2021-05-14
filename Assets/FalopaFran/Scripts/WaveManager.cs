using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public int currentWaveNumber = 0;
    public int prevWaveNumber = -1;
    public int currentEnemiesInWave;
    [SerializeField] private int waveEnemyScaler = 2;
    [SerializeField] private int baseEnemiesPerWave;

    [SerializeField] private List<DummySpawner> spawners = new List<DummySpawner>();

    private void Start()
    {
        Main.instance.EventManager.SubscribeToEvent(GameEvent.SpawnCookie, OnSpawn);
        Main.instance.EventManager.SubscribeToEvent(GameEvent.TermoClicked, StartNewWave);
    }

    public bool SpawnersActive()
    {
        Debug.Log(spawners.Any(x => x.active));
        return spawners.Any(x => x.active);
    }

    public void StartNewWave()
    {

        if(prevWaveNumber == currentWaveNumber) return;

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
