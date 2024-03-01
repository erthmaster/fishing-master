using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormPit : MonoBehaviour, IInteractable
{
	public void StartInteraction()
	{
		Debug.Log("Interaction started");
	}

	public void StopInteraction()
	{
		Debug.Log("Interaction ended");
	}
}
