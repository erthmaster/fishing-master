using UnityEngine;

public class Shop : MonoBehaviour, IInteractable
{
	public ShopPanel shopUI;
	public InfoPanel infoPanel;
	
	public void Interact()
	{
		shopUI.Interact();
	}

	public void BecomeInteractTarget()
	{
		infoPanel.BecomeInteractTarget(this);
	}

	public void StopBeingInteractTarget()
	{
		infoPanel.StopBeingInteractTarget();
	}
}