using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "ScriptableObjects/NewUnitAbility/PowerEqualsRightUnit", order = 1)]

public class PowerEqualsRightUnit : UnitAbility
{
    UnitCard rightCard;


    public override void DoInit(UnitCard card)
    {
        // TODO: whenever a card is played, check if adjacent


        card.OnPlayed += OnPlayedTrigger;

        card.OnTryBoostPower += TestHasUnitOnTheRight;
        card.OnTryTakeDamage += TestHasUnitOnTheRight;
    }


    protected override void Trigger()
    {
        bool proceed;
        card.InvokeQueryHasCardOnTheRight(out proceed, out rightCard);
        if (proceed)
        {
            SetPowerEqualToRightCard();
        }
        else
        {
            card.Die();
        }
        
    }

    private void OnPlayedTrigger()
    {
        Trigger();

        if (rightCard != null)
        {
            rightCard.OnDeath += DestroyIfRightCardWasDesotryed;
            rightCard.OnPowerBoost += SetPowerEqualToRightCard;
            rightCard.OnTakeDamage += SetPowerEqualToRightCard;
        }
    }

    private void TestHasUnitOnTheRight(out bool proceed)
    {
        bool hasUnitOnTheRight;
        card.InvokeQueryHasCardOnTheRight(out hasUnitOnTheRight, out rightCard);

        proceed = !hasUnitOnTheRight;
    }


    private void DestroyIfRightCardWasDesotryed(UnitCard rightCard)
    {
        rightCard.OnDeath -= DestroyIfRightCardWasDesotryed;
        rightCard.OnPowerBoost -= SetPowerEqualToRightCard;
        rightCard.OnTakeDamage -= SetPowerEqualToRightCard;

        card.Die();
    }

    private void SetPowerEqualToRightCard()
    {
        card.SetPower(rightCard.Power);
    }

}
