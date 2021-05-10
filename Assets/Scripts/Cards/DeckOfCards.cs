using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckOfCards : MonoBehaviour
{
    [SerializeField] List<CardSettings> deck = new List<CardSettings>();
    [SerializeField] int numberOfMaxCards = 5;
    [SerializeField] Card cardModel;

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
        for (int i = 0; i < numberOfMaxCards + 1; i++)
        {
            var card = Instantiate(cardModel, transform).GetComponent<Card>();
            currentDeck.Add(card);
            card.gameObject.SetActive(false);
        }
    }

    void SelectRandomCard()
    {
        CardSettings randomCard = deck[Random.Range(0, deck.Count)];
        deck.Remove(randomCard);
        Card nextCard = currentDeck[0];
        nextCard.gameObject.SetActive(true);
        currentDeck.Remove(nextCard);
        currentCards.Add(nextCard);
        nextCard.transform.position = positions[currentCards.Count - 1];
        nextCard.Initialize(nextCard.transform.position, this, randomCard);
    }

    public void OnUseCard(Card card, CardSettings settings)
    {
        MoveCards(currentCards.IndexOf(card));
        currentCards.Remove(card);
        SelectRandomCard();
        card.gameObject.SetActive(false);
        currentDeck.Add(card);
        deck.Add(settings);
    }

    void MoveCards(int index)
    {
        for (int i = index + 1; i < currentCards.Count; i++)
        {
            currentCards[i].SetPosition(positions[i - 1]);
        }
    }
}
