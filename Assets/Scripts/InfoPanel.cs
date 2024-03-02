using System;
using System.Collections.Generic;
using AurumGames.Animation;
using AurumGames.Animation.Tracks;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class InfoPanel : MonoBehaviour
{
	public Transform target;
	
	private StatedAnimationPlayer<Visibility> _animation;
	
	private void Awake()
	{
		var canvasGroup = GetComponent<CanvasGroup>();

		Vector3 localPosition = target.localPosition;
		var show = new TracksEvaluator(new ITrack[]
		{
			new CanvasGroupOpacityTrack(canvasGroup, FloatTrack.KeyFrames01(new TransitionStruct(300, Easing.QuadOut))),
			new LocalPositionTrack(target, new TransitionStruct(300, Easing.QuintOut).GetKeyFrames(localPosition - new Vector3(0, 1f), localPosition))
		});

		var hide = new TracksEvaluator(new ITrack[]
		{
			new CanvasGroupOpacityTrack(canvasGroup, FloatTrack.KeyFrames10(new TransitionStruct(300, Easing.QuadOut))),
			new LocalPositionTrack(target, new TransitionStruct(300, Easing.QuintOut).GetKeyFrames(localPosition, localPosition - new Vector3(0, 1f)))
		});

		_animation = new StatedAnimationPlayer<Visibility>(this, new Dictionary<Visibility, TracksEvaluator>()
		{
			{ Visibility.Visible, show },
			{ Visibility.Hidden, hide }
		});
		
		_animation.SetStateInstant(Visibility.Hidden);
	}

	public void BecomeInteractTarget()
	{
		_animation.SetState(Visibility.Visible);
	}

	public void StopBeingInteractTarget()
	{
		_animation.SetState(Visibility.Hidden);
	}
}