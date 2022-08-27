using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }


    public void SetTextContent(string content)
    {
        text.text = content;
    }


}
