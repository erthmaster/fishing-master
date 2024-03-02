using System;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public bool blockMovement;
    
    [SerializeField] private float moveSpeed = 3;

    private int WormsAmount
    {
        get => wormsAmount;
        set
        {
            wormsAmount = value;
            wormsAmountText.text = $"Worms: {wormsAmount} / {maxWormsAmount}";
        }
    }
    [SerializeField] private int maxWormsAmount = 20;

    [Header("UI")] 
    [SerializeField] private TextMeshProUGUI wormsAmountText;
    [SerializeField] private int wormsAmount;

    public IInteractable Target
    {
        get => _target;
        set
        {
            _target?.StopBeingInteractTarget();
            _target = value;
            _target?.BecomeInteractTarget();
        }
    }

    private IInteractable _target;
    private Rigidbody2D _rb;

    private void Awake()
    {
        Instance ??= this;

        WormsAmount = wormsAmount;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public int GetWorms(int required)
    {
        if (WormsAmount < required)
        {
            required = WormsAmount;
        }

        WormsAmount -= required;
        return required;
    }

    public void CollectWorms(int amount)
    {
        WormsAmount += amount;

        if (WormsAmount > maxWormsAmount)
            WormsAmount = maxWormsAmount;
    }

    private void Update()
    {
        if (GameManager.Instance.isPlaying)
        {
            if (blockMovement == false)
            {
                var inputX = Input.GetAxis("Horizontal");
                var inputY = Input.GetAxis("Vertical");
                _rb.velocity = new Vector2(inputX * moveSpeed, inputY * moveSpeed);
            }
            else
            {
                _rb.velocity = Vector2.zero;
            }

            if (Target != null)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Target.Interact();
                }
            }
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
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
