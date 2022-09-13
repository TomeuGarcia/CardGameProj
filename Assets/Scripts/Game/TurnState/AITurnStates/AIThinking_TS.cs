using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIThinking_TS : TurnState
{
    private bool wasTurnPassed;
    private float timer;
    private float waitTime = 0.5f;

    private CardQueue aiCardQueue;
    private AICardSequence aiCardSequence;



    public AIThinking_TS(GameObserver gameObserver) : base(gameObserver)
    {
        aiCardQueue = gameObserver.aiCardQueue;
        aiCardSequence = gameObserver.aiCardSequence;
    }


    protected override void DoInit()
    {
        InvokeOnStateInit("AI Thinking");

        wasTurnPassed = false;
        timer = 0.0f;


        QueueTurnCards();
        WarnUpcomingTurnCards();
    }

    public override bool Finish()
    {
        return false;
    }

    public override bool Update()
    {
        timer += Time.deltaTime;


        if (timer > waitTime)
        {
            wasTurnPassed = true;
        }


        if (wasTurnPassed)
        {
            nextState = States.TurnEnd;
            return true;
        }

        return false;
    }


    private void QueueTurnCards()
    {
        gameObserver.QueueTurnCards(aiCardSequence, aiCardQueue);
    }

    private void WarnUpcomingTurnCards()
    {
        gameObserver.WarnUpcomingTurnCards(aiCardSequence);
    }

}