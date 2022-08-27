using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnEnd : PlayerTurnState
{
    private bool hasTurnEndFinished;

    public PlayerTurnEnd(GameObserver gameObserver) : base(gameObserver)
    {

    }


    protected override void DoInit()
    {
        hasTurnEndFinished = false;

        //gameObserver.DisableHandCardSelection(); // (Enabled at PlayerThinking)

        // Trigger Turn End Abilities

        gameObserver.ResolveBoard();
    }

    public override void Finish()
    {
    }

    public override bool Update()
    {
        hasTurnEndFinished = true;
        if (hasTurnEndFinished) { 
            nextState = States.TurnStart;
            return true;
        }

        return false;
    }


}