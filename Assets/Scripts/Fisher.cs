using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fisher : MonoBehaviour
{
    public int wormsAmount;

    [Header("UI")] 
    private TextMesh wormsText;

    void Update()
    {
        wormsText.text = $"Worms: {wormsAmount}";
    }

    public void Show()
    {
        wormsText.gameObject.SetActive(true);
    }

    public void Hide()
    {
        wormsText.gameObject.SetActive(false);
    }
}
