using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurnStartBoostIfOpposed : UnitAbility
{
    [SerializeField, Min(0)] private int boostAmount = 1;


    public override void DoInit(UnitCard card)
    {
        card.OnPlayed += Trigger;
    }


    protected override void Trigger()
    {
        bool proceed;
        card.InvokeQueryIsCardOpposed(out proceed);
        if (proceed)
        {
            card.BoostPower(boostAmount);
        }
        
    }




}
