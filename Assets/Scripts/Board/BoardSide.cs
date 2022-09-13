using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoardSide : MonoBehaviour
{
    [SerializeField] public Vector3 attackForward = Vector3.forward;

    public int score { get; private set; }
    [SerializeField] private TextMeshPro scoreText;
    [SerializeField] private Image scoreImage;

    [SerializeField] public BoardSlot[] boardSlots;

    public int BoardSlotsCount => boardSlots.Length;


    public delegate void BoardSideAction(float value);
    public static event BoardSideAction OnScoreUp;




    private void Awake()
    {
        Init();
    }

    protected void Init()
    {
        UpdateScoreText();

        for (int i = 0; i < boardSlots.Length; ++i)
        {
            boardSlots[i].Init(i);
        }
    }


    public bool IsFull()
    {
        foreach (BoardSlot boardSlot in boardSlots)
        {
            if (!boardSlot.HasCard) return false;
        }

        return true;
    }

    protected void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }

    private IEnumerator ProgressivelyAddScoreText(int addAmount)
    {
        for (int i = 0; i < addAmount; ++i)
        {
            ++score;
            UpdateScoreText();

            scoreImage.fillAmount = score / 10.0f;

            float t = (addAmount - i - 1.0f) / addAmount;

            if (OnScoreUp != null) OnScoreUp(1f + (1 - t) * 0.2f);

            yield return new WaitForSeconds(0.2f + (0.2f * t));
        }
    }

    public void AddUnitPointsToScore(UnitCard card)
    {
        StartCoroutine(ProgressivelyAddScoreText(card.Power));
    }

    protected void AddPointsToScore(int addAmount)
    {
        StartCoroutine(ProgressivelyAddScoreText(addAmount));
    }

    private IEnumerator ProgressivelySubtractScoreText(int subtractAmount)
    {
        int absAmount = -subtractAmount;
        for (int i = subtractAmount; i < 0; ++i)
        {
            --score;
            UpdateScoreText();

            scoreImage.fillAmount = score / 10.0f;

            float t = (absAmount - i - 1.0f) / absAmount;
            yield return new WaitForSeconds(0.02f + (0.02f * t));
        }
    }

    protected void SubtractPointsToScore(int subtractAmount)
    {
        StartCoroutine(ProgressivelySubtractScoreText(-subtractAmount));
    }


    public void PlayCardAtBoardSlotI(UnitCard card, int boardSlotI)
    {
        PlayCardAtBoardSlot(card, boardSlots[boardSlotI]);
    }

    protected void PlayCardAtBoardSlot(UnitCard card, BoardSlot boardSlot)
    {
        boardSlot.SetCard(card);

        SubscribeToQueries(card);     

        card.cardTransform.InstantMoveToMeshPosition();
        card.cardTransform.Rotate(Quaternion.identity, 0.1f);
        card.cardTransform.MoveToPosition(boardSlot.CardPosition, 0.1f);
    }




    // Queries
    private void SubscribeToQueries(UnitCard card)
    {
        card.OnDeath += UnsubscribeToQueries;

        card.OnQueryHasCardOnTheRight += CheckHasCardOnTheRight;
    }

    private void UnsubscribeToQueries(UnitCard card)
    {
        card.OnDeath -= UnsubscribeToQueries;

        card.OnQueryHasCardOnTheRight -= CheckHasCardOnTheRight;
    }


    private void CheckHasCardOnTheRight(out bool hasCardOnTheRight, int helperId, out UnitCard rightCard)
    {
        hasCardOnTheRight = false;
        rightCard = null;

        if (helperId + 1 < BoardSlotsCount)
        {
            hasCardOnTheRight = boardSlots[helperId + 1].HasCard;
            if (hasCardOnTheRight)
            {
                rightCard = boardSlots[helperId + 1].card;
            }
        }

    }

}
