using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] private List<Card> cards;

    public bool IsEmpty => cards.Count == 0;

    public bool testingIsClicked = false;

    private void Awake()
    {
        Shuffle();
        ArrangeCards();
    }

    private void OnMouseDown()
    {
        testingIsClicked = true;
    }
    private void OnMouseUp()
    {
        testingIsClicked = false;
    }


    public void AddCard(ref Card card)
    {
        cards.Add(card);

        card.cardTransform.DisableCollider();
    }

    public Card DrawTopCard()
    {
        return DrawCard(0);
    }

    private Card DrawCard(int cardI)
    {
        Card card = cards[cardI];
        cards.RemoveAt(cardI);

        card.cardTransform.EnableCollider();
        return card;
    }


    public void Shuffle()
    {
        List<Card> shuffledCards = new List<Card>();

        for (int i = cards.Count; i > 0; --i)
        {
            int randomI = Random.Range(0, cards.Count);

            shuffledCards.Add(cards[randomI]);
            cards.RemoveAt(randomI);
        }

        cards = shuffledCards;
    }

    private void PrintCards()
    {
        for (int i = 0; i < cards.Count; ++i)
        {
            Debug.Log(i.ToString() + ". " + cards[i].name);
        }
    }

    private void ArrangeCards()
    {
        float length = cards.Count;
        float gapSizeY = 0.05f;
        float startDisplacement = length * gapSizeY + gapSizeY / 2.0f;
        

        for (int i = cards.Count - 1; i >= 0; --i)
        {
            Vector3 endPos = transform.position + new Vector3(0, startDisplacement - (i * gapSizeY), 0);
            cards[i].cardTransform.MoveToPosition(endPos, 0.3f);
        }

    }

}
