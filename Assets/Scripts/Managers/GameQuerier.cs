using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameQuerier
{
    private BoardSide playerBoardSide, opposingBoardSide;

    public GameQuerier(BoardSide playerBoardSide, BoardSide opposingBoardSide)
    {
        this.playerBoardSide = playerBoardSide;
        this.opposingBoardSide = opposingBoardSide;

        
        Card.OnQueryIsOpposed += CheckCardIsOpposed;
    }

    ~GameQuerier()
    {
        Card.OnQueryIsOpposed -= CheckCardIsOpposed;
    }


    private void CheckCardIsOpposed(out bool isOpposed, int boardSlotI)
    {
        isOpposed = playerBoardSide.boardSlots[boardSlotI].HasCard && opposingBoardSide.boardSlots[boardSlotI].HasCard;
    }

}
