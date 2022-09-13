using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpposingBoardSide : BoardSide
{
    [SerializeField] private BoardSlotWarning[] boardSlotWarnings;

    public void EnableWarning(int index)
    {
        boardSlotWarnings[index].EnableWarning();
    }

    public void DisableWarning(int index)
    {
        boardSlotWarnings[index].DisableWarning();
    }

}
