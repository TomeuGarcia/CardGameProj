using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCardCastTarget : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Color originalColor;


    public delegate void SpecialCardCastTargetAction();
    public event SpecialCardCastTargetAction OnPlayerMouseClickDown;

    bool castEnabled;



    private void Awake()
    {
        originalColor = spriteRenderer.color;
    }

    private void OnMouseDown()
    {
        if (!castEnabled) return;

        if (OnPlayerMouseClickDown != null) OnPlayerMouseClickDown();
        LightOff();
    }

    private void OnMouseEnter()
    {
        if (!castEnabled) return; 
        
        LightOn();
    }

    private void OnMouseExit()
    {
        if (!castEnabled) return; 
        
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



    public void EnableCasting()
    {
        castEnabled = true;
    }
    public void DisableCasting()
    {
        castEnabled = false;
    }

}
