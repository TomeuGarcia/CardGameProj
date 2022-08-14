using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassButton : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] new Collider collider;

    public delegate void PassButtonAction();
    public event PassButtonAction OnPass;


    private void OnMouseDown()
    {
        if (OnPass != null) OnPass();

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
