using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitCard : Card
{
    [Header("Power & Cost")]
    [SerializeField] TextMeshPro costText;
    [SerializeField] TextMeshPro powerText;

    public int cost = 1;
    public int basePower = 1;
    public int Power { get; private set; }
    public bool IsDead => Power <= 0;
    public bool IsBoosted => Power > basePower;
    public bool IsDamaged => Power < basePower;



    public delegate void UnitCardAction(UnitCard unitCard);
    public static event UnitCardAction OnCardCanBePlayedQuery;
    public event UnitCardAction OnSurvivedAttackerDamage;
    public event UnitCardAction OnDeath;

    public event CardAction OnTakeDamage;
    public event CardAction OnPowerBoost;

    public delegate void UnitCardTryAction(out bool proceed);
    public event UnitCardTryAction OnTryTakeDamage;
    public event UnitCardTryAction OnTryBoostPower;

    public delegate void UnitCardQueryAction(out bool proceed, int helperId, out UnitCard rightCard);
    public event UnitCardQueryAction OnQueryHasCardOnTheRight;



    protected override void Init()
    {
        cardType = CardType.Unit;

        ResetPower();
        UpdateCostText();
    }

    public override void Play()
    {
        HideCost();

        InvokeOnPlayed();
    }

    public override void QueryIfCanBePlayed()
    {
        if (OnCardCanBePlayedQuery != null) OnCardCanBePlayedQuery(this);
    }

    public override void AddAbility(IAbility ability)
    {
        cardAbilitiesDisplayer.AddAbilityAtSortedPosition(ability);
    }


    private void ResetPower()
    {
        Power = basePower;
        UpdatePowerText();
    }

    public void SetPower(int power)
    {
        bool powerIncremented = Power < power;

        Power = power;
        UpdatePowerText();

        if (powerIncremented) cardShaderAnimator.Boost();
        else cardShaderAnimator.TakeDamage();
    }

    public void BoostPower(int amount)
    {
        bool proceed = true;
        if (OnTryBoostPower != null) OnTryBoostPower(out proceed);
        if (!proceed) return;

        Power += amount;
        UpdatePowerText();

        if (OnPowerBoost != null) OnPowerBoost();
        cardShaderAnimator.Boost();
    }

    public bool DamagePower(int amount) // returns true if died
    {
        bool proceed = true;
        if (OnTryTakeDamage != null) OnTryTakeDamage(out proceed);
        if (!proceed) return false;


        Power -= amount;
        UpdatePowerText();
        if (IsDead)
        {
            Die();
        }

        if (OnTakeDamage != null) OnTakeDamage();
        cardShaderAnimator.TakeDamage();

        return IsDead;
    }

    public void TakeDamageFromUnitCard(int amount, UnitCard attacker)
    {
        if (!DamagePower(amount))
        {
            if (OnSurvivedAttackerDamage != null) OnSurvivedAttackerDamage(attacker); 
        }
    }


    public void Die()
    {
        InvokeOnDeath();
        StartCoroutine(DoDie());        
    }

    private IEnumerator DoDie()
    {
        float duration = 0.75f;
        cardShaderAnimator.Destroy(duration);
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }


    private void UpdatePowerText()
    {
        powerText.text = Power.ToString();
        powerText.color = IsBoosted ? CardHelper.boostedColor : IsDamaged ? CardHelper.damagedColor : CardHelper.baseColor;
    }

    private void UpdateCostText()
    {
        costText.text = cost.ToString();
    }

    private void HideCost()
    {
        costText.gameObject.SetActive(false);
    }


    protected void InvokeOnDeath()
    {
        if (OnDeath != null) OnDeath(this);
    }

    public void InvokeQueryHasCardOnTheRight(out bool proceed, out UnitCard rightCard)
    {
        proceed = false;
        rightCard = null;
        if (OnQueryHasCardOnTheRight != null) OnQueryHasCardOnTheRight(out proceed, helperId, out rightCard);
    }

}
