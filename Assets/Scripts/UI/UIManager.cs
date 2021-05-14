using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    [SerializeField] Image manaBar = null;
    [SerializeField] Image lifeBar = null;

    [SerializeField] RectTransform bubbleParent = null;
    [SerializeField] Image bubbleModel = null;
    [SerializeField] TextMeshProUGUI waveNumber = null;
    [SerializeField] TextMeshProUGUI scoreTxt = null;
    [SerializeField] UIDialog ui_Dialog = null;

    Image[] bubbles = new Image[0];

    [Header("Score")]
    [SerializeField] UIDialog scoreScreen = null;
    [SerializeField] TextMeshProUGUI wavesCompleted = null;
    [SerializeField] TextMeshProUGUI enemiesKilled = null;
    [SerializeField] TextMeshProUGUI finalCombo = null;
    [SerializeField] TextMeshProUGUI finalScore = null;
    [SerializeField] GameObject highScoreGO = null;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Main.instance.EventManager.SubscribeToEvent(GameEvent.StartNewWave, RefreshWaveCounter);
    }

    private void RefreshWaveCounter(object[] parametercontainer)
    {
        Debug.Log("se hace");
        string number = parametercontainer[0].ToString();

        waveNumber.text = $"Ola {number}";
    }

    public void RefreshScoreUI(int currentScore)
    {
        scoreTxt.text = "Puntaje: " + currentScore.ToString();
    }

    public void CreateManaBar(int maxMana)
    {
        bubbles = new Image[maxMana];
        for (int i = 0; i < maxMana; i++)
        {
            var newBubble = Instantiate(bubbleModel, bubbleParent);
            bubbles[i] = newBubble;
        }
    }

    public void ChangeManaBar(int manaAmmount)
    {
        for (int i = 0; i < bubbles.Length; i++)
        {
            if (i < manaAmmount && !bubbles[i].gameObject.activeSelf)
                bubbles[i].gameObject.SetActive(true);
            else if (i >= manaAmmount && bubbles[i].gameObject.activeSelf)
                bubbles[i].gameObject.SetActive(false);
        }
    }

    public void ChangeLifeBar(float percent)
    {
        lifeBar.fillAmount = percent;
    }

    public void DisplayDialog(string txt)
    {
        ui_Dialog.Open(txt);
    }

    public void FinalScreen(int waves, int enemies, int combo, int score, bool isHighscore)
    {
        wavesCompleted.text = waves.ToString();
        enemiesKilled.text = enemies.ToString();
        finalCombo.text = combo.ToString();
        finalScore.text = score.ToString();

        highScoreGO.SetActive(isHighscore);

        scoreScreen.Open("Puntaje");
    }

    public void RestartFunction()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
