using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Fisher : MonoBehaviour
{
    public int wormsAmount;
    public int maxWormsAmount;

    [Header("UI")] 
    [SerializeField] private TextMeshPro wormsText;

    void Update()
    {
        wormsText.text = $"Worms:\n{wormsAmount} / {maxWormsAmount}";
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
