using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyBoardSlot : BoardSlot
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Color originalColor;


    public delegate void BoardSlotAction(BoardSlot thisBoardSlot);
    public event BoardSlotAction OnPlayerMouseClickDown;



    private void Awake()
    {
        originalColor = spriteRenderer.color;
    }

    private void OnMouseDown()
    {
        if (OnPlayerMouseClickDown != null) OnPlayerMouseClickDown(this);
    }

    private void OnMouseEnter()
    {
        if (!HasCard)
        {
            LightOn();
        }
    }

    private void OnMouseExit()
    {
        LightOff();
    }



    private void LightOn()
    {
        spriteRenderer.color = originalColor + Color.white * 0.2f;
    }

    private void LightOff()
    {
        spriteRenderer.color = originalColor;
    }


}
