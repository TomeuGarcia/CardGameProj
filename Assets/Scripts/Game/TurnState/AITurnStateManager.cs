using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurnStateManager : TurnStateManager
{

    public override void Init(GameObserver gameObserver)
    {
        turnStates = new Dictionary<TurnState.States, TurnState>();
        turnStates.Add(TurnState.States.TurnStart, new AITurnStart_TS(gameObserver));
        turnStates.Add(TurnState.States.Thinking, new AIThinking_TS(gameObserver));
        turnStates.Add(TurnState.States.TurnEnd, new AITurnEnd_TS(gameObserver));
        turnStates.Add(TurnState.States.EndToBeginTransition, new EndToBeginTransition_TS(gameObserver));

        currentTurnState = TurnState.States.TurnStart;
        turnStates[currentTurnState].Init();
    }

}
