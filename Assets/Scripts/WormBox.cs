using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class WormBox : MonoBehaviour
{
    public int amount;
    public TextMeshPro amountText;
    
    private void Start()
    {
        amountText.text = amount.ToString();
    }
    
    public void Interact()
    {
        Player.Instance.CollectWorms(amount);
        Destroy(gameObject);
    }
}
