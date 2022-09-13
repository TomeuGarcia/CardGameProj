using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnStart_TS : TurnState
{
    public PlayerTurnStart_TS(GameObserver gameObserver) : base(gameObserver)
    {

    }


    protected override void DoInit()
    {
        InvokeOnStateInit("Player Turn Start");

        // Trigger Turn Start Abilities
        gameObserver.FriendlyTurnStart();
    }

    public override bool Finish()
    {
        return false;
    }

    public override bool Update()
    {

        if (!gameObserver.CanDrawCards())
        {
            gameObserver.cameraManager.SetHandCamera();
            nextState = States.Thinking;
            return true;
        }

        nextState = States.DrawCard;
        return true;
    }
}
