using UnityEngine;

public class Shop : MonoBehaviour, IInteractable
{
	public ShopPanel shopUI;
	
	public void Interact()
	{
		shopUI.Interact();
	}

	public void BecomeInteractTarget()
	{
		
	}

	public void StopBeingInteractTarget()
	{
		
	}
}