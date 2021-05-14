using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem instance { get; private set; }
    int currentScore = 0;
    SaveState save;
    int currentCombo = 0;
    int maxCombo = 0;
    int wavesCompleted;
    int enemiesKilled;
    bool inCombo;
    float timer;

    [SerializeField, Range(1,10)] int comboMultiplier = 2;
    [SerializeField] float comboTimeLimit = 3;
    [SerializeField] int scorePerWave = 100;
    public const string saveStateName = "SaveState";

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Main.instance.EventManager.SubscribeToEvent(GameEvent.StartNewWave, RefreshWaveCount);
        Main.instance.EventManager.SubscribeToEvent(GameEvent.MateDead, EndGame);
        UIManager.instance.RefreshScoreUI(currentScore);
        if (BinarySerialization.IsFileExist(saveStateName))
        {
            save = BinarySerialization.Deserialize<SaveState>(saveStateName);
        }
        else
        {
            save = new SaveState();
            BinarySerialization.Serialize(saveStateName, save);
        }
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
        UIManager.instance.RefreshScoreUI(currentScore);
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
        currentScore += scorePerWave * wavesCompleted;
    }

    void EndGame()
    {
        OverComboTime();
        bool highScore = false;
        if(save.highscore < currentScore)
        {
            save.highscore = currentScore;
            BinarySerialization.Serialize(saveStateName, save);
            highScore = true;
        }

        UIManager.instance.FinalScreen(wavesCompleted, enemiesKilled, maxCombo, currentScore, highScore);
    }
}
