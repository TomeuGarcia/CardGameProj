using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "ScriptableObjects/NewSpecialAbility", order = 2)]
public abstract class SpecialAbility : IAbility
{
    protected SpecialCard card;

    public override void Init(Card card)
    {
        this.card = (SpecialCard)card;
        DoInit(this.card);
    }

    public abstract void DoInit(SpecialCard card);
}

