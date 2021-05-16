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
    [SerializeField] Image perfilFace = null;
    [SerializeField] Sprite[] faceSprites = new Sprite[0];
    [SerializeField] float percentToFirstFace = 0.6f;
    [SerializeField] float percentToSecondFace = 0.3f;
    [SerializeField] UIDialog cardsToDiscardWarning = null;

    [SerializeField] Image[] bubbles = new Image[0];

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
            if (i < manaAmmount && bubbles[i].color.a == 0)
                bubbles[i].color = new Color(bubbles[i].color.r, bubbles[i].color.g, bubbles[i].color.b, 1);
            else if (i >= manaAmmount && bubbles[i].color.a == 1)
                bubbles[i].color = new Color(bubbles[i].color.r, bubbles[i].color.g, bubbles[i].color.b, 0);
        }
    }

    public void ChangeLifeBar(float percent)
    {
        Debug.Log("a ver de donde llamo");
        lifeBar.fillAmount = percent;

        if (percent < percentToSecondFace)
            perfilFace.sprite = faceSprites[2];
        else if (percent < percentToFirstFace)
            perfilFace.sprite = faceSprites[1];
        else
            perfilFace.sprite = faceSprites[0];
    }

    public void DisplayDialog(string txt)
    {
        ui_Dialog.Open(txt);
    }

    public void FinalScreen(int waves, int enemies, int combo, int score, bool isHighscore)
    {
        Main.instance.GetMainCharacter.GetComponent<Frano.CharController>().InputsOn();
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

    public void DiscardCardsDisclaimer(int discardAmmount)
    {
        if (discardAmmount != 0)
            cardsToDiscardWarning.Open("Podés descartar " + discardAmmount + " cartas antes de que empiece la nueva ola.");
        else
            cardsToDiscardWarning.Open("Ya no podés descartar más cartas.");
    }

    public void CloseDiscardCardsDisclaimer()
    {
            cardsToDiscardWarning.Close();
    }
}
