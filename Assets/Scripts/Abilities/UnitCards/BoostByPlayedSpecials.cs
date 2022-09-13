using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewAbility", menuName = "ScriptableObjects/NewUnitAbility/BoostByPlayedSpecials", order = 1)]
public class BoostByPlayedSpecials : UnitAbility
{
    private int numSpecialCardsPlayedThisTurn;


    public override void DoInit(UnitCard card)
    {
        ResetBoost();

        card.OnPlayed += Trigger;   
        SpecialCard.OnSpecialPlayed += IncrementBoost;
        card.OnTurnEnd += ResetBoost;
    }


    protected override void Trigger()
    {
        card.BoostPower(numSpecialCardsPlayedThisTurn);
    }


    private void IncrementBoost()
    {
        ++numSpecialCardsPlayedThisTurn;
    }

    private void ResetBoost()
    {
        numSpecialCardsPlayedThisTurn = 0;
    }


}
