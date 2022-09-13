using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewAbility", menuName = "ScriptableObjects/NewUnitAbility/SpawnSpecialOnDeath", order = 1)]
public class SpawnSpecialOnDeath : UnitAbility
{
    [SerializeField] GameObject specialCardPrefab;


    public override void DoInit(UnitCard card)
    {
        card.OnDeath += SpawnDeathCard;
    }


    protected override void Trigger()
    {
        CardSpawner.SpawnCardInHand(specialCardPrefab);
    }

    private void SpawnDeathCard(Card thisCard)
    {
        Trigger();
    }


}
