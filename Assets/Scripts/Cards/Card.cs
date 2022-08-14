using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    [SerializeField] public CardTransform cardTransform;

    [SerializeField] private Ability[] abilities;

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

        foreach (Ability ability in abilities)
        {
            ability.Init(this);
        }
    }

    protected virtual void TestInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (OnTurnEnd != null) OnTurnEnd();
        }
    }


    protected abstract void Init();

    public virtual void Play() // make abstract
    {
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }



}
