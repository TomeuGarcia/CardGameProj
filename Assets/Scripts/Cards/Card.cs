using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    [Header("Card Identity")]
    [SerializeField] private new string name;
    [SerializeField] private Sprite cardSprite;

    [Header("Shader")]
    [SerializeField] protected CardShaderAnimator cardShaderAnimator;

    [Header("Card Transformations")]
    [SerializeField] public CardTransform cardTransform;

    [Header("Abilities")]
    [SerializeField] protected CardAbilitiesDisplayer cardAbilitiesDisplayer;
    [SerializeField] protected IAbility[] abilities;

    public enum CardType { Unit, Special };
    public CardType cardType { get; protected set; }

    [HideInInspector] public int helperId;
    [HideInInspector] public bool isFriendly;


    public delegate void CardAction();
    public event CardAction OnPlayed;
    public event CardAction OnTurnStart;
    public event CardAction OnTurnEnd;

    public delegate void CardQueryAction(out bool proceed, int helperId);
    public static event CardQueryAction OnQueryIsOpposed;




    private void OnValidate()
    {
        Init();
        cardShaderAnimator.SetCardSprite(cardSprite); // uncomment to apply changes
        cardShaderAnimator.SetCardName(name);
    }

    private void Awake()
    {
        cardTransform.Init(this);
        Init();

        for (int i = 0; i < abilities.Length; ++i)
        {
            abilities[i] = Instantiate(abilities[i]);
            abilities[i].Init(this);

            AddAbility(abilities[i]);
        }

        cardShaderAnimator.Init();
        cardShaderAnimator.SetCardSprite(cardSprite);
        cardShaderAnimator.SetCardName(name);
    }


    protected abstract void Init();

    public abstract void Play();

    public abstract void QueryIfCanBePlayed();

    public abstract void AddAbility(IAbility ability);

    protected void InvokeOnPlayed()
    {
        if (OnPlayed != null) OnPlayed();
    }

    public void InvokeOnTurnStart()
    {
        if (OnTurnStart != null) OnTurnStart();
    }

    public void InvokeOnTurnEnd()
    {
        if (OnTurnEnd != null) OnTurnEnd();
    }

    public void InvokeQueryIsCardOpposed(out bool proceed)
    {
        proceed = false;
        if (OnQueryIsOpposed != null) OnQueryIsOpposed(out proceed, helperId);
    }

}
