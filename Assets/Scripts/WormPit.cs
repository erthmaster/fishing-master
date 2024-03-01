using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WormPit : MonoBehaviour, IInteractable
{
	public TextMeshPro title;
	public int wormsAmount;

	private void Start()
	{
		title.gameObject.SetActive(false);
	}
	
	public void Interact()
	{
		Player.Instance.CollectWorms(wormsAmount);
	}

	public void BecomeInteractTarget()
	{
		title.gameObject.SetActive(true);
	}

	public void StopBeingInteractTarget()
	{
		title.gameObject.SetActive(false);
	}
}
