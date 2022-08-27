using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    [SerializeField] public CardTransform cardTransform;

    [SerializeField] protected Ability[] abilities;

    public enum CardType { Unit, Special };
    public CardType cardType { get; protected set; }


    public delegate void CardAbilityAction();
    public event CardAbilityAction OnPlayed;
    public event CardAbilityAction OnTurnStart;
    public event CardAbilityAction OnTurnEnd;




    private void OnValidate()
    {
        Init();
    }

    private void Awake()
    {
        Init();

        for (int i = 0; i < abilities.Length; ++i)
        {
            abilities[i] = Instantiate(abilities[i]);
            abilities[i].Init(this);
        }
    }


    protected abstract void Init();

    public abstract void Play();

    public abstract void QueryIfCanBePlayed();

    protected void InvokeOnPlayed()
    {
        if (OnPlayed != null) OnPlayed();
    }

}
