using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyBoardSide : BoardSide
{
    [SerializeField] private SpecialCardCastTarget specialCardCastTarget;


    public bool IsBoardSlotSelected => selectedBoardSlot != null;
    private BoardSlot selectedBoardSlot;
    public UnitCard UnitInSelectedBoardSlot => selectedBoardSlot.card;

    public delegate void BoardAction();
    public event BoardAction OnUnitCardDemand;
    public event BoardAction OnSpecialCardDemand;



    private void Awake()
    {
        UpdateScoreText();
        GainEnergySpecialAbility.OnEnergyGain += AddPointsToScore;
    }



    // Play Unit Cards
    public void EnablePlayUnitCards()
    {
        foreach (FriendlyBoardSlot boardSlot in boardSlots)
        {
            boardSlot.OnPlayerMouseClickDown += DemandUnitCardForBoardSlot;
        }
    }
    public void DisablePlayUnitCards()
    {
        foreach (FriendlyBoardSlot boardSlot in boardSlots)
        {
            boardSlot.OnPlayerMouseClickDown -= DemandUnitCardForBoardSlot;
        }
    }

    private void DemandUnitCardForBoardSlot(BoardSlot boardSlot)
    {
        if (boardSlot.HasCard)
        {
            return;
        }

        selectedBoardSlot = boardSlot;

        if (OnUnitCardDemand != null) OnUnitCardDemand();
    }


    public void PlayCardInSelectedBoardSlot(UnitCard card)
    {
        selectedBoardSlot.card = card;

        card.cardTransform.InstantMoveToMeshPosition();
        card.cardTransform.Rotate(Quaternion.identity, 0.1f);
        card.cardTransform.MoveToPosition(selectedBoardSlot.CardPosition, 0.1f);

        AddPointsToScore(-card.cost);

        selectedBoardSlot = null;
    }



    // Play Special Cards
    public void EnablePlaySpecialCards()
    {
        specialCardCastTarget.EnableCasting();
        specialCardCastTarget.OnPlayerMouseClickDown += DemandSpecialCard;

        foreach (FriendlyBoardSlot boardSlot in boardSlots)
        {
            boardSlot.OnPlayerMouseClickDown += DemandSpecialCardForBoardSlot;
        }
    }
    public void DisablePlaySpecialCards()
    {
        specialCardCastTarget.DisableCasting();
        specialCardCastTarget.OnPlayerMouseClickDown -= DemandSpecialCard;

        foreach (FriendlyBoardSlot boardSlot in boardSlots)
        {
            boardSlot.OnPlayerMouseClickDown -= DemandSpecialCardForBoardSlot;
        }
    }


    private void DemandSpecialCardForBoardSlot(BoardSlot boardSlot)
    {
        if (!boardSlot.HasCard)
        {
            return;
        }

        selectedBoardSlot = boardSlot;

        DemandSpecialCard();
    }


    private void DemandSpecialCard()
    {
        if (OnSpecialCardDemand != null) OnSpecialCardDemand();
    }


    public void PlaySpecialCard(SpecialCard card)
    {
        card.cardTransform.InstantMoveToMeshPosition();
        card.cardTransform.Rotate(Quaternion.identity, 0.1f);
        card.cardTransform.MoveToPosition(specialCardCastTarget.transform.position + Vector3.down, 0.1f);
    }

    public void PlaySpecialCardOnUnit(SpecialCard card)
    {
        card.cardTransform.InstantMoveToMeshPosition();
        card.cardTransform.Rotate(Quaternion.identity, 0.1f);
        card.cardTransform.MoveToPosition(selectedBoardSlot.CardPosition + Vector3.down, 0.1f);

        selectedBoardSlot = null;
    }



}
