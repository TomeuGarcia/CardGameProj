using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoardSide : MonoBehaviour
{
    public int score { get; private set; }
    [SerializeField] private TextMeshPro scoreText;
    [SerializeField] private Image scoreImage;

    [SerializeField] protected BoardSlot[] boardSlots;
    




    private void Awake()
    {
        UpdateScoreText();        
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

    private IEnumerator ProgressivelyUpdateScoreText(int addAmount)
    {
        for (int i = 0; i < Mathf.Abs(addAmount); ++i)
        {
            score += addAmount > 0 ? 1 : -1;
            UpdateScoreText();

            scoreImage.fillAmount = score / 10.0f;

            float t = (addAmount - i - 1.0f) / addAmount;
            yield return new WaitForSeconds(0.02f + (0.02f * t));
        }
    }

    public void AddUnitPointsToScore()
    {
        int addAmount = 0;
        foreach (BoardSlot boardSlot in boardSlots)
        {
            if (boardSlot.HasCard) addAmount += boardSlot.card.Power;
        }

        StartCoroutine(ProgressivelyUpdateScoreText(addAmount));
    }

    protected void AddPointsToScore(int addAmount)
    {
        StartCoroutine(ProgressivelyUpdateScoreText(addAmount));
    }



}
