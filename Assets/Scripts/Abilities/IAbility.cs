using UnityEngine;

public abstract class IAbility : ScriptableObject
{
    public GameObject abilityDisplayPrefab;


    public abstract void Init(Card card);

    protected abstract void Trigger();
}
