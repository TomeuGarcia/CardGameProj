using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrawCard : PlayerTurnState
{
    private bool wasCardDrawn;

    public PlayerDrawCard(GameObserver gameObserver) : base(gameObserver)
    {
        
    }

    ~PlayerDrawCard()
    {
        
    }


    protected override void DoInit()
    {
        InvokeOnStateInit("Draw Card");

        wasCardDrawn = false;
        gameObserver.EnableDrawCardFromDecks();
        gameObserver.OnCardDrawn += SetWasCardDrawnTrue;
    }

    public override void Finish()
    {
        gameObserver.DisableDrawCardFromDecks();
        gameObserver.OnCardDrawn -= SetWasCardDrawnTrue;
    }

    public override bool Update()
    {
        if (wasCardDrawn)
        {
            nextState = States.Thinking;
            return true;
        }

        return false;
    }


    private void SetWasCardDrawnTrue()
    {
        wasCardDrawn = true;
    }
}
