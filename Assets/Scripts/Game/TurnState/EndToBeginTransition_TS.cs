using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndToBeginTransition_TS : TurnState
{
    private bool proceedBegin;


    public EndToBeginTransition_TS(GameObserver gameObserver) : base(gameObserver)
    {

    }


    protected override void DoInit()
    {
        InvokeOnStateInit("AI EndToBegin Transition");

        proceedBegin = false;
    }

    public override bool Finish()
    {
        return false;
    }

    public override bool Update()
    {
        proceedBegin = true;
        if (proceedBegin)
        {
            nextState = States.TurnStart;
            return true;
        }

        return false;
    }
}
