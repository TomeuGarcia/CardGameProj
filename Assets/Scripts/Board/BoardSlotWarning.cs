using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSlotWarning : MonoBehaviour
{
    [SerializeField] private GameObject warning;

    private void Awake()
    {
        DisableWarning();
    }


    public void EnableWarning()
    {
        warning.SetActive(true);
    }

    public void DisableWarning()
    {
        warning.SetActive(false);
    }

}
