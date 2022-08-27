using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnStart : PlayerTurnState
{
    public PlayerTurnStart(GameObserver gameObserver) : base(gameObserver)
    {

    }


    protected override void DoInit()
    {
        // Trigger Turn Start Abilities
    }

    public override void Finish()
    {
        
    }

    public override bool Update()
    {
        if (!gameObserver.CanDrawCards())
        {
            nextState = States.Thinking;
            return true;
        }

        nextState = States.DrawCard;
        return true;
    }
}
