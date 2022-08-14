using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private List<Card> cards;
    private int selectedCardI = -1;

    public bool HasSelectedCard => selectedCardI != -1;
    public Card SelectedCard => cards[selectedCardI]; 

    private static Vector3 selectedDirection = new Vector3(2.0f, 0.0f, 2.0f);
    private static Vector3 hoveredDirection = new Vector3(0.0f, 0.0f, 0.2f);

    bool testingPlayedCard = false;


    public bool HandUpdate(out Card card)
    {
        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    DecrementSelectedCard();
        //    ArrangeCards();
        //}
        //else if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    IncrementSelectedCard();
        //    ArrangeCards();
        //}

        //if (Input.GetKeyDown(KeyCode.UpArrow) && cards.Count > 0)
        //{
        //    PlaySelectedCard();
        //    DecrementSelectedCard();
        //    ArrangeCards();
        //    return true;
        //}

        if (Input.GetMouseButton(1) && selectedCardI != -1)
        {
            ArrangeUnhoveredCard(selectedCardI);
            UnselectCard();
            ArrangeCards();
        }

        if (testingPlayedCard)
        {
            testingPlayedCard = false;
            card = cards[selectedCardI];
            return true;
        }



        card = null;
        return false;
    }

    private void EnableCardEvents(Card card)
    {
        card.cardTransform.OnPlayerMouseEnter += ArrangeHoveredCard;
        card.cardTransform.OnPlayerMouseExit += ArrangeUnhoveredCard;
        card.cardTransform.OnPlayerMouseClickDown += SelectCard;
        card.cardTransform.OnPlayerMouseClickDown += ArrangeSelectedCard;
        //card.cardTransform.OnPlayerMouseClickDown += TryPlayCard;
    }

    private void DisableCardEvents(Card card)
    {
        card.cardTransform.OnPlayerMouseEnter -= ArrangeHoveredCard;
        card.cardTransform.OnPlayerMouseExit -= ArrangeUnhoveredCard;
        card.cardTransform.OnPlayerMouseClickDown -= SelectCard;
        card.cardTransform.OnPlayerMouseClickDown -= ArrangeSelectedCard;
        //card.cardTransform.OnPlayerMouseClickDown -= TryPlayCard;
    }




    public void AddCard(Card card)
    {
        cards.Add(card);
        card.cardTransform.id = cards.Count - 1;
        EnableCardEvents(card);
        ArrangeCards();
    }

    public void PlayLeftmostCard()
    {
        PlayCard(0);
    }

    public void PlaySelectedCard()
    {
        PlayCard(selectedCardI);
    }

    private void PlayCard(int cardI)
    {
        Card card = cards[cardI];
        cards.RemoveAt(selectedCardI);

        ArrangeCards();

        DisableCardEvents(card);
        card.Play();
    }



    private void TryPlayCard(int cardI)
    {
        return;
        selectedCardI = cardI;
        testingPlayedCard = true;

        // TODO check if can play card
        if (true) return;


        PlayCard(cardI);
        ArrangeCards();
    }


    public void ArrangeCards()
    {
        float length = cards.Count;
        float gapSizeX = 0.75f;
        float startDisplacement = -length / 2.0f + gapSizeX / 2.0f;

        for (int i = 0; i < cards.Count; ++i)
        {
            Vector3 endPos = transform.position + new Vector3(startDisplacement + (i * gapSizeX), 0, 0);
            cards[i].cardTransform.id = i;
            cards[i].cardTransform.MoveToPosition(endPos, 0.1f, true);
            cards[i].cardTransform.Rotate(transform.rotation, 0.3f);
        }
    }

    private void ArrangeHoveredCard(int cardId)
    {
        Vector3 endPos = cards[cardId].cardTransform.Position + transform.TransformDirection(hoveredDirection);
        cards[cardId].cardTransform.MoveMeshToPosition(endPos, 0.1f);
    }
    
    private void ArrangeUnhoveredCard(int cardId)
    {
        cards[cardId].cardTransform.MoveMeshToOrigin(0.1f);
    }

    private void ArrangeSelectedCard(int cardId)
    {
        Vector3 endPos = transform.position + transform.TransformDirection(selectedDirection);
        cards[cardId].cardTransform.MoveMeshToPosition(endPos, 0.1f);
    }


    private void SelectCard(int cardId)
    {
        selectedCardI = cardId;
        cards[cardId].cardTransform.OnPlayerMouseExit -= ArrangeUnhoveredCard;
        cards[cardId].cardTransform.OnPlayerMouseEnter -= ArrangeHoveredCard;

        DisableNonSelectedCardsInteraction();
    }

    private void UnselectCard()
    {
        EnableNonSelectedCardsInteraction();

        cards[selectedCardI].cardTransform.OnPlayerMouseExit += ArrangeUnhoveredCard;
        cards[selectedCardI].cardTransform.OnPlayerMouseEnter += ArrangeHoveredCard;
        selectedCardI = -1;
    }


    private void EnableNonSelectedCardsInteraction()
    {
        for (int i = 0; i < selectedCardI; ++i)
        {
            EnableCardEvents(cards[i]);
        }
        for (int i = selectedCardI + 1; i < cards.Count; ++i)
        {
            EnableCardEvents(cards[i]);
        }
    }

    private void DisableNonSelectedCardsInteraction()
    {
        for (int i = 0; i < selectedCardI; ++i)
        {
            DisableCardEvents(cards[i]);
        }
        for (int i = selectedCardI + 1; i < cards.Count; ++i)
        {
            DisableCardEvents(cards[i]);
        }
    }




}
