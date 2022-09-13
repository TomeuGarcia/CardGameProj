using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurnEnd_TS : TurnState
{
    private bool isBoardFinishedResolving;


    public AITurnEnd_TS(GameObserver gameObserver) : base(gameObserver)
    {

    }


    protected override void DoInit()
    {
        InvokeOnStateInit("AI Turn End");

        isBoardFinishedResolving = false;

        // Trigger Turn End Abilities
        gameObserver.ResolveOpponentBoard();
        gameObserver.OnBoardFinishedResolving += SetIsBoardFinishedResolving;
    }

    public override bool Finish()
    {
        gameObserver.OnBoardFinishedResolving -= SetIsBoardFinishedResolving;

        return true;
    }

    public override bool Update()
    {
        if (isBoardFinishedResolving)
        {
            nextState = States.EndToBeginTransition;
            return true;
        }

        return false;
    }


    private void SetIsBoardFinishedResolving()
    {
        isBoardFinishedResolving = true;
    }

}