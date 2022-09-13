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
        Init();
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
        PlayCardAtBoardSlot(card, selectedBoardSlot);
        SubtractPointsToScore(card.cost);

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


    private void PlaySpecialCardAtTarget(SpecialCard card, Vector3 targetPos, float translationDuration = 0.1f, float animationDuration = 0.5f)
    {
        card.cardTransform.InstantMoveToMeshPosition();
        card.cardTransform.Rotate(Quaternion.identity, translationDuration);
        card.cardTransform.MoveToPosition(targetPos, translationDuration);

        StartCoroutine(PlayedSpecialCardAnimation(card, translationDuration, animationDuration));
    }

    private IEnumerator PlayedSpecialCardAnimation(SpecialCard card, float waitDuration, float animationDuration)
    {
        yield return new WaitForSeconds(waitDuration);
        card.PlayFadeAnimation(animationDuration);
        yield return new WaitForSeconds(animationDuration);
        card.gameObject.SetActive(false);
    }


    public void PlaySpecialCard(SpecialCard card)
    {
        PlaySpecialCardAtTarget(card, specialCardCastTarget.transform.position + Vector3.up * 0.5f, 0.1f, 0.5f);
    }

    public void PlaySpecialCardOnUnit(SpecialCard card)
    {
        PlaySpecialCardAtTarget(card, selectedBoardSlot.CardPosition + Vector3.up * 0.5f, 0.1f, 0.5f);

        selectedBoardSlot = null;
    }



}
