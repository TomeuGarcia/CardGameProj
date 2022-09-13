using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSlot : MonoBehaviour
{
    public UnitCard card { get; protected set; }

    protected int id;

    public bool HasCard => card != null;
    public Vector3 CardPosition => transform.position;// + (Vector3.up * 0.3f);


    public void Init(int id)
    {
        this.id = id;
    }

    public void SetCard(UnitCard unitCard)
    {
        card = unitCard;
        card.helperId = id;
    }

}
