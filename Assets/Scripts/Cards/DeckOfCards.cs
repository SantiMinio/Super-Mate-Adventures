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

    public CardSettings lastCardUsed = null;
    float timeToUsedLastCard = 0;

    private void Update()
    {
        timeToUsedLastCard += Time.deltaTime;
    }

    private void Start()
    {
        privateInstance = this;
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
        nextCard.transform.position = posToSpawn.position;
        nextCard.Initialize(this, randomCard);
        nextCard.GoToPos(positions[currentCards.Count - 1]);
    }

    public void OnUseCard(Card card, CardSettings settings)
    {
        MoveCards(currentCards.IndexOf(card));
        currentCards.Remove(card);
        SelectRandomCard();
        card.GoToPos(posToSpawn.position, () =>
        {
            card.gameObject.SetActive(false);
        });
        currentDeck.Add(card);
        deck.Add(settings);

        lastCardUsed = settings;
        Debug.Log(lastCardUsed.title);
        timeToUsedLastCard = 0;
    }
    public static DeckOfCards privateInstance;

    void MoveCards(int index)
    {
        for (int i = index + 1; i < currentCards.Count; i++)
        {
            currentCards[i].GoToPos(positions[i - 1]);
        }
    }

    public static float GetTimeToUseLastCard() => privateInstance.TimeToUseLastCard();

    public float TimeToUseLastCard() => timeToUsedLastCard;

    public static CardSettings GetLastCard() => privateInstance.LastCard();

    public CardSettings LastCard() => lastCardUsed;
}
