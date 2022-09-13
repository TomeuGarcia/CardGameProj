using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TurnState
{
    public enum States { None, TurnStart, DrawCard, Thinking, HoldingSelectedCard, TurnEnd, EndToBeginTransition }

    public States nextState { get; protected set; } = States.None;

    public delegate void PlayerTurnStateAction(string HUDtext);
    public static event PlayerTurnStateAction OnStateInit;


    protected GameObserver gameObserver;


    protected TurnState(GameObserver gameObserver)
    {
        this.gameObserver = gameObserver;
    }

    public void Init()
    {
        nextState = States.None;
        
        DoInit();
    }

    protected abstract void DoInit();
    public abstract bool Finish(); // returns true if turn switches to next "player"
    public abstract bool Update();


    protected void InvokeOnStateInit(string HUDtext)
    {
        if (OnStateInit != null) OnStateInit(HUDtext);
    }

}
