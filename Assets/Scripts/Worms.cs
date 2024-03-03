using System;
using AurumGames.Animation;
using AurumGames.Animation.Tracks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Worms : MonoBehaviour
{
	public TextMeshProUGUI text;

	public int WormsAmount
	{
		get => _wormsAmount;
		set
		{
			_wormsAmount = value;

			if (_animation != null)
			{
				_track.Set(value);
				_animation.Update();
			}
		}
	}

	[SerializeField] private int _wormsAmount;

	private FluentFloatTrack _track;
	private FluentAnimationPlayer _animation;

	private void Awake()
	{
		_track = new FluentFloatTrack(_wormsAmount, value =>
		{
			text.text = value.ToString("F0");
		}, new Transition(300, Easing.QuadOut));

		_animation = new FluentAnimationPlayer(this, _track);
		WormsAmount = _wormsAmount;
	}
}