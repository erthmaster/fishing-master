using System;
using System.Collections.Generic;
using AurumGames.Animation;
using AurumGames.Animation.Tracks;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class InfoPanel : MonoBehaviour
{
	public GameObject interactKey;
	private StatedFluentAnimationPlayer<Visibility> _animation;
	
	private void Awake()
	{
		var canvasGroup = GetComponent<CanvasGroup>();
		_animation = canvasGroup.Fade(this);
		
		_animation.SetStateInstant(Visibility.Hidden);
	}

	public void BecomeInteractTarget(IInteractable interactable)
	{
		interactKey.SetActive(Player.Instance.Target == interactable);
			
		
		if (_animation?.CurrentState == Visibility.Hidden)
			_animation.SetState(Visibility.Visible);
	}

	public void StopBeingInteractTarget()
	{
		if (_animation?.CurrentState == Visibility.Visible)
			_animation.SetState(Visibility.Hidden);
	}
}