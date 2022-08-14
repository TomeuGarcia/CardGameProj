using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSlot : MonoBehaviour
{
    public UnitCard card { get; set; }


    public bool HasCard => card != null;
    public Vector3 CardPosition => transform.position;// + (Vector3.up * 0.3f);


    public delegate void BoardSlotAction(BoardSlot thisBoardSlot);
    public event BoardSlotAction OnPlayerMouseClickDown;


    private void OnMouseDown()
    {
        if (OnPlayerMouseClickDown != null) OnPlayerMouseClickDown(this);
    }



    public void LightOn()
    {
        // TODO
    }

    public void LightOff()
    {
        // TODO
    }


}
