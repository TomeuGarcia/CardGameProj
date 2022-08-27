using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitCard : BoardCard
{
    [SerializeField] TextMeshPro costText;
    [SerializeField] TextMeshPro powerText;

    public int cost = 1;
    public int basePower = 1;
    public int Power { get; private set; }
    public bool IsDead => Power <= 0;
    public bool IsBoosted => Power > basePower;
    public bool IsDamaged => Power < basePower;



    public delegate void UnitCardAction(UnitCard thisUnitCard);
    public static event UnitCardAction OnCardCanBePlayedQuery;


    protected override void Init()
    {
        cardType = CardType.Unit;

        // randomize power (testing)
        basePower = Random.Range(1, 5);
        cost = Random.Range(1, basePower);

        ResetPower();
        UpdateCostText();
    }

    public override void Play()
    {
        //Destroy(gameObject);
        //gameObject.SetActive(false);
    }

    public override void QueryIfCanBePlayed()
    {
        if (OnCardCanBePlayedQuery != null) OnCardCanBePlayedQuery(this);
    }

    private void ResetPower()
    {
        Power = basePower;
        UpdatePowerText();
    }



    public void BoostPower(int amount)
    {
        Power += amount;
        UpdatePowerText();
    }

    public void DamagePower(int amount)
    {
        Power -= amount;
        UpdatePowerText();
        if (IsDead)
        {
            Die();
        }
    }

    public void Die()
    {
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

}
