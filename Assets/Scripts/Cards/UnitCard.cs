using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitCard : BoardCard
{
    [SerializeField] TextMeshPro powerText;

    public int basePower = 1;
    public int Power { get; private set; }
    public bool IsDead => Power <= 0;
    public bool IsBoosted => Power > basePower;
    public bool IsDamaged => Power < basePower;


    protected override void Init()
    {
        ResetPower();
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

}
