using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHoldingSelectedCard_TS : TurnState
{
    private bool wasCardPlayed;
    private bool wasCardUnselected;

    private Card.CardType selectedCardType;


    public PlayerHoldingSelectedCard_TS(GameObserver gameObserver) : base(gameObserver)
    {
        gameObserver.OnHoldingUnitCard += () => selectedCardType = Card.CardType.Unit;
        gameObserver.OnHoldingSpecialCard += () => selectedCardType = Card.CardType.Special;
    }


    protected override void DoInit()
    {
        InvokeOnStateInit("Holding Selected Card"); 

        wasCardPlayed = false;
        wasCardUnselected = false;

        gameObserver.DisablePassButtonInteraction();
        gameObserver.EnableStopHoldingSelectedCard();


        if (selectedCardType == Card.CardType.Unit)
            gameObserver.EnablePlayUnitCardsOnBoard();
        else if (selectedCardType == Card.CardType.Special)
            gameObserver.EnablePlaySpecialCards();

        gameObserver.OnCardPlayed += SetWasCardPlayedTrue;
        gameObserver.OnStopHoldingSelectedCard += SetWasCardUnselectedTrue;
    }

    public override bool Finish()
    {
        gameObserver.EnablePassButtonInteraction();
        gameObserver.DisableStopHoldingSelectedCard();


        if (selectedCardType == Card.CardType.Unit)
            gameObserver.DisablePlayUnitCardsOnBoard();
        else if (selectedCardType == Card.CardType.Special)
            gameObserver.DisablePlaySpecialCards();

        gameObserver.OnCardPlayed -= SetWasCardPlayedTrue;
        gameObserver.OnStopHoldingSelectedCard -= SetWasCardUnselectedTrue;

        return false;
    }

    public override bool Update()
    {
        if (wasCardPlayed)
        {
            nextState = States.Thinking;
            return true;
        }
        if (wasCardUnselected)
        {
            nextState = States.Thinking;
            return true;
        }

        return false;
    }


    private void SetWasCardPlayedTrue()
    {
        wasCardPlayed = true;
    }

    private void SetWasCardUnselectedTrue()
    {
        wasCardUnselected = true;
    }


}