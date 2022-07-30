using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "ScriptableObjects/NewUnitAbility", order = 1)]
public abstract class UnitAbility : Ability
{
    protected UnitCard card;

    public override void Init(Card card)
    {
        this.card = (UnitCard)card;
        DoInit(this.card);
    }

    public abstract void DoInit(UnitCard card);
}

