using System;
using UnityEngine;
using UnityEngine.UI;

public class FisherImageGrabber : MonoBehaviour
{
	public Fisher fisher;
	public Image self;

	private void Awake()
	{
		self = GetComponent<Image>();
	}

	private void Update()
	{
		if (fisher == null)
			return;
		
		self.sprite = fisher.renderer.sprite;
	}
}