using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCard : Card
{
    public delegate void SpecialCardAction(SpecialCard thisSpecialCard);
    public static event SpecialCardAction OnCardCanBePlayedQuery;

    public delegate void SpecialCardAction2();
    public static event SpecialCardAction2 OnSpecialPlayed;


    [HideInInspector] public bool isTargetingUnitCard;
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
        InvokeOnSpecialPlayed();
    }

    public override void QueryIfCanBePlayed()
    {
        if (OnCardCanBePlayedQuery != null) OnCardCanBePlayedQuery(this);
    }

    public override void AddAbility(IAbility ability)
    {
        //cardAbilitiesDisplayer.AddAbilityAtCenteredPosition(ability);
        //cardAbilitiesDisplayer.AddAbilityAtSortedPosition(ability);
    }


    public void SetTargetingUnitCard(UnitCard unitCard)
    {
        isTargetingUnitCard = true;
        targetedUnitCard = unitCard;
    }

    public void PlayFadeAnimation(float duration)
    {
        cardShaderAnimator.Fade(duration);
    }


    private void InvokeOnSpecialPlayed()
    {
        if (OnSpecialPlayed != null) OnSpecialPlayed();
    }

}
