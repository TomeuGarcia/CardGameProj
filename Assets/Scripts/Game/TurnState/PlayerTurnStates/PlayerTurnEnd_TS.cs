using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnEnd_TS : TurnState
{
    private bool hasTurnEndFinished;
    private bool isBoardFinishedResolving;

    private float turnEndTimer;
    private const float turnEndTime = 0.5f;


    public PlayerTurnEnd_TS(GameObserver gameObserver) : base(gameObserver)
    {

    }


    protected override void DoInit()
    {
        InvokeOnStateInit("Player Turn End");

        hasTurnEndFinished = false;
        isBoardFinishedResolving = false;
        turnEndTimer = 0f;

        //gameObserver.DisableHandCardSelection(); // (Enabled at PlayerThinking)

        gameObserver.OnBoardFinishedResolving += SetIsBoardFinishedResolving;
    }

    public override bool Finish()
    {
        gameObserver.OnBoardFinishedResolving -= SetIsBoardFinishedResolving;

        return true;
    }

    public override bool Update()
    {
        turnEndTimer += Time.deltaTime;
        if (!hasTurnEndFinished && turnEndTimer > turnEndTime)
        {
            hasTurnEndFinished = true;

            gameObserver.cameraManager.SetBoardCamera();
            gameObserver.ResolveFriendlyBoard();
        }

        
        if (isBoardFinishedResolving) { 
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