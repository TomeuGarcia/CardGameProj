using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewCardSequence", menuName = "ScriptableObjects/CardSequence", order = 1)]
public class AICardSequence : ScriptableObject
{
    [System.Serializable]
    public struct Play
    {
        public int boardSlotIndex;
        public GameObject cardPrefab;
    }


    [System.Serializable]
    public struct TurnCardSequence
    {
        public int turn;
        public Play[] plays;
    }


    public bool looping = false;
    [SerializeField] private TurnCardSequence[] cardSequence;
    
    private int currentTurn;
    private int lastTurn;


    private Dictionary<int, Play[]> sortedCardSequence;

    public void Init()
    {
        lastTurn = 0;

        sortedCardSequence = new Dictionary<int, Play[]>();
        foreach (TurnCardSequence turnCardSequence in cardSequence)
        {
            sortedCardSequence.Add(turnCardSequence.turn, turnCardSequence.plays);

            if (turnCardSequence.turn > lastTurn) lastTurn = turnCardSequence.turn;
        }

        currentTurn = 0;
    }

    public void IncrementTurn()
    {
        currentTurn = looping ? ++currentTurn % (lastTurn + 1) : ++currentTurn;
    }

    public bool GetCurrentTurnPlays(out Play[] plays)
    {
        bool sequenceContainsCurrentTurn = sortedCardSequence.ContainsKey(currentTurn);

        if (sequenceContainsCurrentTurn)
        {
            plays = sortedCardSequence[currentTurn];
        }
        else
        {
            plays = null;
        }        

        return sequenceContainsCurrentTurn;
    }

    public bool GetCurrentTurnBoardSlotIndicesPlays(out int[] boardSlotIndices)
    {
        bool sequenceContainsCurrentTurn = sortedCardSequence.ContainsKey(currentTurn);

        if (sequenceContainsCurrentTurn)
        {
            List<int> indices = new List<int>();
            foreach (Play play in sortedCardSequence[currentTurn])
            {
                indices.Add(play.boardSlotIndex);
            }
            boardSlotIndices = indices.ToArray();
        }
        else
        {
            boardSlotIndices = null;
        }

        return sequenceContainsCurrentTurn;
    }

}
