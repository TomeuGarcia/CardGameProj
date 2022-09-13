using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassButton : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] new Collider collider;

    public delegate void PassButtonAction();
    public event PassButtonAction OnPass;

    public static event PassButtonAction OnPassPlayAudio;

    [HideInInspector] public bool canPass = false;


    private void OnMouseDown()
    {
        if (canPass)
            Pass(); 
    }

    private void Pass()
    {
        if (OnPass != null) OnPass();
        if (OnPassPlayAudio != null) OnPassPlayAudio();

        animator.SetTrigger("Pass");
    }


    public void EnablePassing()
    {
        collider.enabled = true;
    }

    public void DisablePassing()
    {
        collider.enabled = false;
    }

}
