using System;
using System.Collections.Generic;
using AurumGames.Animation;
using AurumGames.Animation.Tracks;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(CanvasGroup))]
public class PausePanel : MonoBehaviour
{
	public static PausePanel Instance;
	
	public AudioMixerSnapshot current;
	public AudioMixerSnapshot off;
	public TextMeshProUGUI audioText;
	public bool audioOn;
	
	private StatedFluentAnimationPlayer<Visibility> _animation;
	
	private void Awake()
	{
		Instance ??= this;
		
		var canvasGroup = GetComponent<CanvasGroup>();
		_animation = canvasGroup.Fade(this);
		
		_animation.SetStateInstant(Visibility.Visible);
	}

	public void Play(AudioMixerSnapshot snapshot)
	{
		current = snapshot;

		if (audioOn)
		{
			current.TransitionTo(1);
		}
	}

	public void SwitchAudio()
	{
		audioOn ^= true;
		audioText.text = $"Audio: {(audioOn ? "On" : "Off")}";

		if (audioOn)
		{
			current.TransitionTo(0.5f);
		}
		else
		{
			off.TransitionTo(0.5f);
		}
	}

	public void Show()
	{
		_animation.SetState(Visibility.Visible);
	}

	public void Hide()
	{
		_animation.SetState(Visibility.Hidden);
	}
}