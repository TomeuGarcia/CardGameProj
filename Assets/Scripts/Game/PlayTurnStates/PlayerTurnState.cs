using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerTurnState
{
    public enum States { None, TurnStart, DrawCard, Thinking, HoldingSelectedCard, TurnEnd }

    public States nextState { get; protected set; } = States.None;

    public delegate void PlayerTurnStateAction(string HUDtext);
    public static event PlayerTurnStateAction OnStateInit;

    protected GameObserver gameObserver;


    protected PlayerTurnState(GameObserver gameObserver)
    {
        this.gameObserver = gameObserver;
    }

    public void Init()
    {
        nextState = States.None;
        
        DoInit();
    }

    protected abstract void DoInit();
    public abstract void Finish();
    public abstract bool Update();


    protected void InvokeOnStateInit(string HUDtext)
    {
        if (OnStateInit != null) OnStateInit(HUDtext);
    }

}
