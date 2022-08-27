using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private List<Card> cards;
    
    public Card selectedCard { get; private set; }
    public bool HasSelectedCard => selectedCard != null;


    private static Vector3 selectedDirection = new Vector3(2.0f, 0.0f, 2.0f);
    private static Vector3 hoveredDirection = new Vector3(0.0f, 0.1f, 0.2f);


    public delegate void HandAction();
    public event HandAction OnCardSelected;
    public event HandAction OnCardUnselected;



    private void Update()
    {
        if (Input.GetMouseButton(1) && HasSelectedCard)
        {
            ArrangeUnhoveredCard(selectedCard);
            UnselectCard();
            ArrangeCards();
        }

    }

    private void EnableArrangeCardEvents(Card card)
    {
        card.cardTransform.OnPlayerMouseEnter += ArrangeHoveredCard;
        card.cardTransform.OnPlayerMouseExit += ArrangeUnhoveredCard;
        
        //card.cardTransform.OnPlayerMouseClickDown += TryPlayCard;
    }

    private void DisableArrangeCardEvents(Card card)
    {
        card.cardTransform.OnPlayerMouseEnter -= ArrangeHoveredCard;
        card.cardTransform.OnPlayerMouseExit -= ArrangeUnhoveredCard;

        //card.cardTransform.OnPlayerMouseClickDown -= TryPlayCard;
    }


    private void QueryCardCanBePlayed(Card card)
    {
        card.QueryIfCanBePlayed();
    }

    public void ProceedSelectCardToPlay(Card card)
    {
        SelectCard(card);
        ArrangeSelectedCard(card);
    }

    private void EnableSelectionCardEvents(Card card)
    {
        card.cardTransform.OnPlayerMouseClickDown += QueryCardCanBePlayed;
        //card.cardTransform.OnPlayerMouseClickDown += SelectCard;
        //card.cardTransform.OnPlayerMouseClickDown += ArrangeSelectedCard;
    }

    private void DisableSelectionCardEvents(Card card)
    {
        card.cardTransform.OnPlayerMouseClickDown -= QueryCardCanBePlayed; 
        //card.cardTransform.OnPlayerMouseClickDown -= SelectCard;
        //card.cardTransform.OnPlayerMouseClickDown -= ArrangeSelectedCard;
    }

    public void EnableCardsSelection()
    {
        foreach (Card card in cards)
        {
            EnableSelectionCardEvents(card);
        }
    }

    public void DisableCardsSelection()
    {
        foreach (Card card in cards)
        {
            DisableSelectionCardEvents(card);
        }
    }




    public void AddCard(Card card)
    {
        cards.Add(card);
        EnableArrangeCardEvents(card);
        //ArrangeCards();
    }


    public void RemoveSelectedCard()
    {
        cards.Remove(selectedCard);

        //DisableArrangeCardEvents(selectedCard);
        EnableNonSelectedCardsInteraction();
        ArrangeCards();

        selectedCard = null;
    }




    public void ArrangeCards()
    {
        float length = cards.Count;
        float ratio = Mathf.Min(5.0f / length, 1.0f);

        float gapSizeX = 0.65f * ratio;

        float startDisplacementX = (-length * ratio / 2.0f) + (gapSizeX / 2.0f);

        float gapSizeY = 0.02f;

        Vector3 displacement = Vector3.right * startDisplacementX;

        float fanAngle = 20.0f;
        float startRotation = (-fanAngle / 2.0f);
        Vector3 fanRotationStep = Vector3.up * (fanAngle / length);
        Vector3 fanRotation = Vector3.up * startRotation - fanRotationStep / 2.0f;

        for (int i = 0; i < cards.Count; ++i)
        {
            fanRotation += fanRotationStep;
            Quaternion fan = Quaternion.Euler(fanRotation);
            cards[i].cardTransform.Rotate(transform.rotation * fan, 0.1f);

            displacement += Vector3.right * gapSizeX;
            Vector3 endPos = transform.position + displacement;
            endPos += transform.up * gapSizeY * i;
            endPos += -transform.forward * Mathf.Sin(Mathf.Deg2Rad * fanRotation.magnitude);            
            cards[i].cardTransform.MoveToPosition(endPos, 0.1f, true);
        }
    }

    private void ArrangeHoveredCard(Card card)
    {
        Vector3 endPos = card.cardTransform.Position + transform.TransformDirection(hoveredDirection);
        card.cardTransform.MoveMeshToPosition(endPos, 0.1f);
    }
    
    private void ArrangeUnhoveredCard(Card card)
    {
        card.cardTransform.MoveMeshToOrigin(0.1f);
    }

    private void ArrangeSelectedCard(Card card)
    {
        Vector3 endPos = transform.position + transform.TransformDirection(selectedDirection);
        card.cardTransform.MoveMeshToPosition(endPos, 0.1f);
    }


    private void SelectCard(Card card)
    {
        selectedCard = card;

        DisableNonSelectedCardsInteraction();

        card.cardTransform.OnPlayerMouseExit -= ArrangeUnhoveredCard;
        card.cardTransform.OnPlayerMouseEnter -= ArrangeHoveredCard;


        if (OnCardSelected != null) OnCardSelected();
    }

    private void UnselectCard()
    {
        if (OnCardUnselected != null) OnCardUnselected();

        EnableNonSelectedCardsInteraction();

        selectedCard.cardTransform.OnPlayerMouseExit += ArrangeUnhoveredCard;
        selectedCard.cardTransform.OnPlayerMouseEnter += ArrangeHoveredCard;
        
        selectedCard = null;
    }


    private void EnableNonSelectedCardsInteraction()
    {
        for (int i = 0; i < cards.Count; ++i)
        {
            if (cards[i] == selectedCard) continue;
            EnableArrangeCardEvents(cards[i]);
            //EnableSelectionCardEvents(cards[i]);
        }
    }

    private void DisableNonSelectedCardsInteraction()
    {
        for (int i = 0; i < cards.Count; ++i)
        {
            if (cards[i] == selectedCard)
            {
                continue;
            }
            DisableArrangeCardEvents(cards[i]);
            DisableSelectionCardEvents(cards[i]);
        }
    }




}
