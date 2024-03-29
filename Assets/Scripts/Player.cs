using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    public static Player Instance;

    public UnityEvent FishAmountChanged;

    public bool blockMovement;
    
    [SerializeField] private float moveSpeed = 3;

    public int WormsAmount
    {
        get => wormsAmount.WormsAmount;
        set => wormsAmount.WormsAmount = value;
    }

    public int FishAmount
    {
        get => fishAmount;
        set
        {
            fishAmount = value;
            FishAmountChanged.Invoke();

            if (fishAmountText)
            {
                fishAmountText.text = fishAmount.ToString();
            }
        }
    }
    
    public int FishPerSecond
    {
        set
        {
            if (fishPerSecondText)
            {
                fishPerSecondText.text = value.ToString();
            }
        }
    }

    [SerializeField] private int fishAmount;
    
    [Header("UI")] 
    [SerializeField] private Worms wormsAmount;
    [SerializeField] private TextMeshProUGUI fishAmountText;
    [SerializeField] private TextMeshProUGUI fishPerSecondText;
    

    public IInteractable Target => _targets.LastOrDefault();

    private IInteractable _target;
    private Rigidbody2D _rb;
    private Animator _animator;
    private readonly List<IInteractable> _targets = new();

    private void Awake()
    {
        Instance ??= this;

        FishAmount = fishAmount;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
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
                _animator.SetBool(IsRunning, inputX != 0 || inputY != 0);
                if (inputX != 0)
                    transform.localScale = new Vector2(inputX * 1.3f, 1.3f);
            }
            else
            {
                _rb.velocity = Vector2.zero;
                _animator.SetBool(IsRunning, false);
            }

            if (Target != null)
            {
                Target.BecomeInteractTarget();
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
            AddTarget(interactable);
        }
        if (other.TryGetComponent(out WormBox wormBox))
        {
            wormBox.Interact();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            RemoveTarget(interactable);
        }
    }

    private void AddTarget(IInteractable interactable)
    {
        Target?.StopBeingInteractTarget();
        _targets.Add(interactable);
    }

    private void RemoveTarget(IInteractable interactable)
    {
        interactable.StopBeingInteractTarget();
        _targets.Remove(interactable);
    }
}
