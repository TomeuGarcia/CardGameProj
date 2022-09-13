using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager
{
    private TurnStateManager[] turnStateManagers;
    private int currentPlayerTurnI = 0;


    public GameStateManager(GameObserver gameObserver)
    {
        currentPlayerTurnI = 0;
        turnStateManagers = new TurnStateManager[2];
        turnStateManagers[0] = new PlayerTurnStateManager();
        turnStateManagers[0].Init(gameObserver);
        turnStateManagers[1] = new AITurnStateManager();
        turnStateManagers[1].Init(gameObserver);
    }

    public void Update()
    {
        if (turnStateManagers[currentPlayerTurnI].Update())
        {
            ProceedNextPlayerTurnI();
        }
    }


    private void ProceedNextPlayerTurnI()
    {
        ++currentPlayerTurnI;
        currentPlayerTurnI %= turnStateManagers.Length;
    }


}
