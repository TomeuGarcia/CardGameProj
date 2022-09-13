using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "ScriptableObjects/NewSpecialAbility/GainEnergy", order = 1)]
public class GainEnergySpecialAbility : SpecialAbility
{
    [SerializeField] private int energyAmount = 1;


    public delegate void GainEnergySpecialAbilityAction(int energyAmount);
    public static event GainEnergySpecialAbilityAction OnEnergyGain;


    public override void DoInit(SpecialCard card)
    {
        card.OnPlayed += Trigger;
    }


    protected override void Trigger()
    {
        if (card.isTargetingUnitCard)
        {
            card.targetedUnitCard.BoostPower(1);
        }
        else
        {
            if (OnEnergyGain != null) OnEnergyGain(energyAmount);
        }
    }

}
