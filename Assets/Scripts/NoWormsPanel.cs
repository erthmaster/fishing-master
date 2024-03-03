using System;
using System.Collections.Generic;
using AurumGames.Animation;
using AurumGames.Animation.Tracks;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class NoWormsPanel : MonoBehaviour
{
	public float jumpAmount;
	
	private StatedFluentAnimationPlayer<Visibility> _animation;
	private AnimationPlayer _jump;
	
	private void Awake()
	{
		var canvasGroup = GetComponent<CanvasGroup>();
		_animation = canvasGroup.Fade(this);
		
		_animation.SetStateInstant(Visibility.Hidden);

		Transform self = transform;
		Vector3 position = self.localPosition;
		_jump = new AnimationPlayer(this, new ITrack[]
		{
			new LocalPositionTrack(self, new []
			{
				new KeyFrame<Vector3>(0, position, Easing.QuadOut),
				new KeyFrame<Vector3>(300, () => position + Vector3.up * jumpAmount, Easing.CubicIn),
				new KeyFrame<Vector3>(350, () => position + Vector3.up * jumpAmount, Easing.CubicIn),
				new KeyFrame<Vector3>(650, position, Easing.CubicIn),
				new KeyFrame<Vector3>(2000, position, Easing.CubicIn),
			})
		})
		{
			Loop = true
		};
	}

	public void Show()
	{
		if (_jump.IsPlaying == false)
			_jump.PlayFromStart();
		
		_animation.SetStateInstant(Visibility.Visible);
	}

	public void Hide()
	{
		_animation.SetStateInstant(Visibility.Hidden);
	}
}