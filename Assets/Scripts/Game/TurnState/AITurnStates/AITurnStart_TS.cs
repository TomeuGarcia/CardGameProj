using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurnStart_TS : TurnState
{
    public AITurnStart_TS(GameObserver gameObserver) : base(gameObserver)
    {

    }


    protected override void DoInit()
    {
        gameObserver.OpponentTurnStart();
    }

    public override bool Finish()
    {
        return false;
    }

    public override bool Update()
    {
        nextState = States.Thinking;
        return true;
    }
}
