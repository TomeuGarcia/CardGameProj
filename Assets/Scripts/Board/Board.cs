using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Board : MonoBehaviour
{
    private int score = 0;
    [SerializeField] private TextMeshPro pointsText;

    [SerializeField] private BoardSlot[] friendlyBoardSlots;
    [SerializeField] private BoardSlot[] opposingBoardSlots;

    public delegate void BoardAction(BoardSlot boardSlot);
    public event BoardAction OnCardDemand;



    private void Awake()
    {
        foreach (BoardSlot boardSlot in friendlyBoardSlots)
        {
            boardSlot.OnPlayerMouseClickDown += BoardSlotIsSelected;
        }

        UpdateBoardPointsText();
    }



    public void AddUpScore()
    {
        foreach (BoardSlot boardSlot in friendlyBoardSlots)
        {
            if (boardSlot.HasCard) score += boardSlot.card.Power;
        }

        UpdateBoardPointsText();
    }

    private void BoardSlotIsSelected(BoardSlot boardSlot)
    {
        if (boardSlot.HasCard)
        {
            return;
        }

        if (OnCardDemand != null) OnCardDemand(boardSlot);
    }


    private void UpdateBoardPointsText()
    {
        pointsText.text = score.ToString();
    }

}
