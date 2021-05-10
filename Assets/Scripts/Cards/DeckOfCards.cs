using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckOfCards : MonoBehaviour
{
    [SerializeField] Card[] deck = new Card[9];
    [SerializeField] int numberOfMaxCards = 5;

    List<Card> currentDeck = new List<Card>();
    List<Card> currentCards = new List<Card>();

    public RectTransform firstPos = null;
    public RectTransform lastPos = null;
    [SerializeField] RectTransform posToSpawn = null;

    Vector3[] positions = new Vector3[0];

    private void Start()
    {
        positions = new Vector3[numberOfMaxCards];
        float spacingBetweenCards = (firstPos.position.x - lastPos.position.x) / (numberOfMaxCards -1);
        Vector3 currentPos = firstPos.position;
        for (int i = 0; i < numberOfMaxCards; i++)
        {
            positions[i] = currentPos;
            currentPos.x -= spacingBetweenCards;
        }

        SpawnCards();

        for (int i = 0; i < numberOfMaxCards; i++)
        {
            SelectRandomCard();
        }
    }

    void SpawnCards()
    {
        for (int i = 0; i < deck.Length; i++)
        {
            var card = Instantiate(deck[i], transform).GetComponent<Card>();
            currentDeck.Add(card);
            card.gameObject.SetActive(false);
        }
    }

    void SelectRandomCard()
    {
        Card randomCard = currentDeck[Random.Range(0, currentDeck.Count)];
        currentDeck.Remove(randomCard);
        randomCard.gameObject.SetActive(true);
        currentCards.Add(randomCard);
        randomCard.transform.position = positions[currentCards.Count - 1];
        randomCard.Initialize(randomCard.transform.position, this);
    }
}
