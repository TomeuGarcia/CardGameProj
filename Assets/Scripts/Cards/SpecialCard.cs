using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCard : Card
{
    public delegate void SpecialCardAction(SpecialCard thisSpecialCard);
    public static event SpecialCardAction OnCardCanBePlayedQuery;


    public bool isTargetingUnitCard;
    public UnitCard targetedUnitCard { get; private set; }

    protected override void Init()
    {
        cardType = CardType.Special;
        isTargetingUnitCard = false;
        targetedUnitCard = null;
    }



    public override void Play()
    {
        InvokeOnPlayed();
    }

    public override void QueryIfCanBePlayed()
    {
        if (OnCardCanBePlayedQuery != null) OnCardCanBePlayedQuery(this);
    }


    public void SetTargetingUnitCard(UnitCard unitCard)
    {
        isTargetingUnitCard = true;
        targetedUnitCard = unitCard;
    }

}
