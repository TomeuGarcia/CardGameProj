using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MyAbility : UnitAbility, IDeployAbility, ITurnEndAbility
{
    IDeployAbility deployAbility;
    ITurnEndAbility turnEndAbility;

    public override void DoInit(UnitCard card)
    {
        deployAbility = this;
        card.OnDeploy += deployAbility.Trigger;

        turnEndAbility = this;
        card.OnTurnEnd += turnEndAbility.Trigger;
    }


    void IDeployAbility.Trigger()
    {
        Debug.Log("Boost self by 4");
        card.BoostPower(4);
    }

    void ITurnEndAbility.Trigger()
    {
        Debug.Log("Damage self by 1");
        card.DamagePower(1);
    }


}
