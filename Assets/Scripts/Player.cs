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

    public IInteractable Target
    {
        get => target;
        set
        {
            target?.StopInteraction();
            target = value;
        }
    }

    private IInteractable target;

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

            if (Target != null)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Target.StartInteraction();
                }

                if (Input.GetKeyUp(KeyCode.E))
                {
                    Target.StopInteraction();
                }
            }
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
        
        wormsAmountText.text = $"Worms: {wormsAmount} / {maxWormsAmount}";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            Target = interactable;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            if (Target == interactable)
                Target = null;
        }
    }
}
