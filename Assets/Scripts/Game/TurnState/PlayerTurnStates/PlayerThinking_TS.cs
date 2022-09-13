using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThinking_TS : TurnState
{
    private bool wasCardSelected;
    private bool wasTurnPassed;

    public PlayerThinking_TS(GameObserver gameObserver) : base(gameObserver)
    {

    }


    protected override void DoInit()
    {
        InvokeOnStateInit("Thinking");

        wasCardSelected = false;
        wasTurnPassed = false;

        gameObserver.EnableHandCardSelection(); // (Disabled at TurnEnd)

        //gameObserver.EnableHoldingSelectedCard();
        gameObserver.EnablePassButtonInteraction();


        gameObserver.OnHoldingSelectedCard += SetWasCardSelectedTrue;
        gameObserver.OnTurnPass += SetWasTurnPassedTrue;
    }

    public override bool Finish()
    {
        gameObserver.DisableHandCardSelection();

        //gameObserver.DisableHoldingSelectedCard();
        gameObserver.DisablePassButtonInteraction();

        gameObserver.OnHoldingSelectedCard -= SetWasCardSelectedTrue;
        gameObserver.OnTurnPass -= SetWasTurnPassedTrue;

        return false;
    }

    public override bool Update()
    {
        if (wasCardSelected)
        {
            nextState = States.HoldingSelectedCard;
            return true;
        }
        if (wasTurnPassed)
        {
            nextState = States.TurnEnd;
            return true;
        }

        return false;
    }


    private void SetWasCardSelectedTrue()
    {
        wasCardSelected = true;
    }

    private void SetWasTurnPassedTrue()
    {
        wasTurnPassed = true;
    }


}