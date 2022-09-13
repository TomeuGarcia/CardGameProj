using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] private List<Card> cards;

    public bool HasCards => cards.Count > 0;


    public delegate void DeckAction(Deck thisDeck);
    public event DeckAction OnCardDrawDemand;
    public event DeckAction OnPlayerMouseClickDown;

    



    private void Awake()
    {
        Shuffle();
        ArrangeCards();
    }

    private void OnMouseDown()
    {
        if (HasCards)
        {
            if (OnCardDrawDemand != null) OnCardDrawDemand(this);
        }            
    }


    public void AddCard(ref Card card)
    {
        cards.Add(card);
    }

    public Card DrawTopCard()
    {
        return DrawCard(0);
    }

    private Card DrawCard(int cardI)
    {
        Card card = cards[cardI];
        cards.RemoveAt(cardI);        

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
        float gapSizeY = 0.02f;
        float startDisplacement = length * gapSizeY + gapSizeY / 2.0f;
        

        for (int i = cards.Count - 1; i >= 0; --i)
        {
            Vector3 endPos = transform.position + new Vector3(0, startDisplacement - (i * gapSizeY), 0);

            //cards[i].cardTransform.SetPosition(endPos);
            //cards[i].cardTransform.PlayDeckShuffleAnimation((length - i) / length);

            cards[i].cardTransform.MoveToPosition(endPos, 0.3f);

            cards[i].cardTransform.Rotate(Quaternion.Euler(0f, Random.Range(-8f, 8f), 0f), 0.3f);
        }

    }

}
