using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardSelector : MonoBehaviour
{
    [SerializeField] CardProbability_Dictionary cards = new CardProbability_Dictionary();
    [SerializeField] int maxProbability = 200;
    [SerializeField] int probabilityPerWave = 5;
    [SerializeField] CardSelectorButton[] buttons = new CardSelectorButton[0];
    [SerializeField] UIDialog animUI = null;

    private void Start()
    {
        Main.instance.EventManager.SubscribeToEvent(GameEvent.KilledAllEnemiesSpawned, OpenCardSelector);
    }

    void OpenCardSelector(params object[] parameters)
    {
        Main.instance.GetMainCharacter.GetComponent<Frano.CharController>().InputsOff();
        int parametersMultiplier = (int)parameters[0] - 1;
        CardProbability_Dictionary newdic = new CardProbability_Dictionary();
        newdic.CopyFrom(cards);

        foreach (var item in cards)
        {
            newdic[item.Key] += probabilityPerWave * parametersMultiplier;
            if (newdic[item.Key] > maxProbability)
                newdic[item.Key] = maxProbability;
        }
        cards = newdic;

        Tuple<CardSettings, int>[] mySelectCards = new Tuple<CardSettings, int>[buttons.Length];

        for (int i = 0; i < buttons.Length; i++)
        {
            var selectCard = RoulletteWheel.Roullette(cards);

            buttons[i].Initialize(selectCard);
            mySelectCards[i] = new Tuple<CardSettings, int>(selectCard, cards[selectCard]);
            cards.Remove(selectCard);
        }

        for (int i = 0; i < mySelectCards.Length; i++) cards.Add(mySelectCards[i].Item1, mySelectCards[i].Item2);

        animUI.Open("Seleccioná una carta");
    }

    public void SelectCard(CardSettings settings)
    {
        Main.instance.GetMainCharacter.GetComponent<Frano.CharController>().InputsOn();
        DeckOfCards.privateInstance.AddCard(settings);
        animUI.Close();
    }
}
