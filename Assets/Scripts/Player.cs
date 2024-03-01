using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Text wormsAmountText;

    private bool atWormPit;
    private bool atFisher;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (GameManager.Instance.isPlaying)
        {
            var inputX = Input.GetAxis("Horizontal");
            var inputY = Input.GetAxis("Vertical");
            _rb.velocity = new Vector2(inputX * moveSpeed, inputY * moveSpeed);

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (atWormPit)
                    wormsAmount = maxWormsAmount;
            }
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
        
        wormsAmountText.text = $"Worms: {wormsAmount} / {maxWormsAmount}";
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<WormPit>())
        {
            atWormPit = true;
        }
        else if (col.TryGetComponent(out Fisher fisher))
        {
            atFisher = true;
            fisher.Show();
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<WormPit>())
        {
            atWormPit = false;
        }
        else if (col.TryGetComponent(out Fisher fisher))
        {
            atFisher = false;
            fisher.Hide();
        }
    }
}
