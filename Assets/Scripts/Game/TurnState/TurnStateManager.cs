using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TurnStateManager
{
    protected TurnState.States currentTurnState;
    protected Dictionary<TurnState.States, TurnState> turnStates;

    public abstract void Init(GameObserver gameObserver);
    
    public bool Update()
    {
        bool finishStateCycle = false;

        if (turnStates[currentTurnState].Update())
        {
            finishStateCycle = turnStates[currentTurnState].Finish();

            currentTurnState = turnStates[currentTurnState].nextState;
            turnStates[currentTurnState].Init();
        }

        return finishStateCycle;
    }



}
