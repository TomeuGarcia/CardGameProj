using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnStateManager : TurnStateManager
{

    public override void Init(GameObserver gameObserver)
    {
        turnStates = new Dictionary<TurnState.States, TurnState>();
        turnStates.Add(TurnState.States.TurnStart, new PlayerTurnStart_TS(gameObserver));
        turnStates.Add(TurnState.States.DrawCard, new PlayerDrawCard_TS(gameObserver));
        turnStates.Add(TurnState.States.Thinking, new PlayerThinking_TS(gameObserver));
        turnStates.Add(TurnState.States.HoldingSelectedCard, new PlayerHoldingSelectedCard_TS(gameObserver));
        turnStates.Add(TurnState.States.TurnEnd, new PlayerTurnEnd_TS(gameObserver));
        turnStates.Add(TurnState.States.EndToBeginTransition, new EndToBeginTransition_TS(gameObserver));

        currentTurnState = TurnState.States.Thinking;
        turnStates[currentTurnState].Init();
    }


}
