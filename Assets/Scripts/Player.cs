using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private int wormsAmount;
    [SerializeField] private int maxWormsAmount = 20;

    [Header("UI")] 
    [SerializeField] private TextMeshPro wormsAmountText;

    public IInteractable Target
    {
        get => _target;
        set
        {
            _target?.StopInteraction();
            _target = value;
        }
    }

    private IInteractable _target;
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
        
        // wormsAmountText.text = $"Worms: {wormsAmount} / {maxWormsAmount}";
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
