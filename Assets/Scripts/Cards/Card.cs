using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    [SerializeField] Ability ability;

    public delegate void CardAbilityAction();
    public event CardAbilityAction OnTurnStart;
    public event CardAbilityAction OnTurnEnd;


    private void OnValidate()
    {
        Init();
    }

    private void Awake()
    {
        ability.Init(this);
        Init();
    }

    protected virtual void TestInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (OnTurnEnd != null) OnTurnEnd();
        }
    }


    protected abstract void Init();


}
