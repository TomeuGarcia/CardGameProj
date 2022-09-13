using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardQueue
{
    private Dictionary<int, Queue<GameObject>> slotMappedCardQueue;
    private GameObserver gameObserver;
    private BoardSide boardSide;
    


    public CardQueue(GameObserver gameObserver, BoardSide boardSide)
    {
        this.gameObserver = gameObserver;
        this.boardSide = boardSide;

        slotMappedCardQueue = new Dictionary<int, Queue<GameObject>>();
        for (int i = 0; i < boardSide.BoardSlotsCount; ++i)
        {
            slotMappedCardQueue.Add(i, new Queue<GameObject>());
        }
    }

    public void QueueCard(GameObject cardPrefab, int boardSlotI)
    {
        slotMappedCardQueue[boardSlotI].Enqueue(cardPrefab);
    }

    public void PlayQueuedUnitCards()
    {
        for (int i = 0; i < boardSide.BoardSlotsCount; ++i)
        {
            if (HasCardToQueue(slotMappedCardQueue[i]) && !boardSide.boardSlots[i].HasCard)
            {
                gameObserver.SpawnAndPlayUnitCard(slotMappedCardQueue[i].Dequeue(), boardSide, i);
                gameObserver.StopWarnUpcomingCardAtBoardSlot(i);
            }
        }

    }


    private bool HasCardToQueue(Queue<GameObject> queue)
    {
        return queue.Count > 0;
    }


}
