using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardSpawner
{
    public delegate void CardSpawnerAction(GameObject cardPrefab, out Card spawnedCard);
    public static event CardSpawnerAction OnSpawnCardInHand;



    public static Card SpawnCardInHand(GameObject cardPrefab)
    {
        Card card = null;

        if (OnSpawnCardInHand != null) OnSpawnCardInHand(cardPrefab, out card);

        return card;
    }


}
