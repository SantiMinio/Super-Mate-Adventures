using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem instance { get; private set; }
    int currentScore = 0;
    int currentCombo = 0;
    int maxCombo = 0;
    int wavesCompleted;
    int enemiesKilled;
    bool inCombo;
    float timer;

    [SerializeField, Range(1,10)] int comboMultiplier = 2;
    [SerializeField] float comboTimeLimit = 3;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Main.instance.EventManager.SubscribeToEvent(GameEvent.StartNewWave, RefreshWaveCount);
    }

    public void RefreshScore(int score)
    {
        if (inCombo)
            score *= comboMultiplier;

        currentScore += score;
        inCombo = true;
        timer = 0;
        currentCombo += 1;
        enemiesKilled += 1;
    }

    private void Update()
    {
        if (!inCombo) return;
        timer += Time.deltaTime;

        if(timer>= comboTimeLimit)
        {
            OverComboTime();
        }
    }

    void OverComboTime()
    {
        inCombo = false;
        timer = 0;
        if (currentCombo > maxCombo)
            maxCombo = currentCombo;
        currentCombo = 0;
    }

    void RefreshWaveCount(params object[] parameters)
    {
        wavesCompleted = (int)parameters[0] - 1;
    }
}
