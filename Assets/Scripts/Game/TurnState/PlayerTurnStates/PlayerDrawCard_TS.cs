using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrawCard_TS : TurnState
{
    private bool wasCardDrawn;

    public PlayerDrawCard_TS(GameObserver gameObserver) : base(gameObserver)
    {
        
    }

    ~PlayerDrawCard_TS()
    {
        
    }


    protected override void DoInit()
    {
        InvokeOnStateInit("Draw Card");

        wasCardDrawn = false;
        gameObserver.EnableDrawCardFromDecks();
        gameObserver.OnCardDrawn += SetWasCardDrawnTrue;

        gameObserver.cameraManager.SetHandCamera();
    }

    public override bool Finish()
    {
        gameObserver.DisableDrawCardFromDecks();
        gameObserver.OnCardDrawn -= SetWasCardDrawnTrue;

        return false;
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
