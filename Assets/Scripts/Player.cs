using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private int wormsAmount;
    [SerializeField] private int maxWormsAmount = 20;

    [Header("UI")] 
    [SerializeField] private TMP_Text wormsAmountText;

    private bool _atWormPit;
    private Fisher _currentFisher;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        var inputX = Input.GetAxis("Horizontal");
        var inputY = Input.GetAxis("Vertical");
        _rb.velocity = new Vector2(inputX * moveSpeed, inputY * moveSpeed);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(_atWormPit)
                wormsAmount = maxWormsAmount;
            if (_currentFisher && (wormsAmount - (_currentFisher.maxWormsAmount - _currentFisher.wormsAmount)) !<= 0)
                _currentFisher.wormsAmount = wormsAmount - (_currentFisher.maxWormsAmount - _currentFisher.wormsAmount); // Non working
        }
        wormsAmountText.text =
            $"Worms: {wormsAmount} /" +
            $" {maxWormsAmount}";
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<WormPit>())
        {
            _atWormPit = true;
        }
        else if (col.GetComponent<Fisher>())
        {
            _currentFisher = col.GetComponent<Fisher>();
            col.GetComponent<Fisher>().ToggleInfo(true);
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<WormPit>())
        {
            _atWormPit = false;
        }
        else if (col.GetComponent<Fisher>())
        {
            _currentFisher = null;
            col.GetComponent<Fisher>().ToggleInfo(false);
        }
    }
}
