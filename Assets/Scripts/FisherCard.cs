using System;
using System.Collections.Generic;
using AurumGames.Animation;
using AurumGames.Animation.Tracks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FisherCard : MonoBehaviour
{
	public Fisher fisher;
	public int id;
	public ShopPanel ShopPanel;

	[Space] 
	public Color normal;
	public Color selected;
	public TextMeshProUGUI fisherName;
	public TextMeshProUGUI wormsTank;
	public TextMeshProUGUI rod;
	public Image panel;

	private StatedAnimationPlayer<bool> _animation;

	private void Awake()
	{
		var selectedTrack = new TracksEvaluator(new ITrack[]
		{
			new ImageColorTrack(panel, new TransitionStruct(300, Easing.QuadOut).GetKeyFrames(normal, selected))
		});
		
		var normalTrack = new TracksEvaluator(new ITrack[]
		{
			new ImageColorTrack(panel, new TransitionStruct(300, Easing.QuadOut).GetKeyFrames(selected, normal))
		});
		
		_animation = new StatedAnimationPlayer<bool>(this, new Dictionary<bool, TracksEvaluator>()
		{
			{ true, selectedTrack },
			{ false, normalTrack }
		});
		
		_animation.SetStateInstant(false);
	}

	public void Bind(Fisher target, int fisherId, ShopPanel shopPanelReference)
	{
		fisher = target;
		id = fisherId;
		
		target.LevelChanged.AddListener(UpdateView);
		target.gameObject.SetActive(true);
		UpdateView();

		ShopPanel = shopPanelReference;
	}
	
	public void OnEnable()
	{
		if (fisher != null)
			UpdateView();
	}

	public void UpdateView()
	{
		fisherName.text = $"Fisher #{id + 1}(Level: {fisher.level})";
		wormsTank.text = $"Worms tank: {fisher.maxWormsAmount}";
		rod.text = $"Fishing rod: {fisher.FPS} FPS";
	}

	public void Unselect()
	{
		_animation.SetState(false);
	}

	public void Select()
	{
		if (_animation.CurrentState == false)
			_animation.SetState(true);
		
		ShopPanel.SetSelectedItem(this);
	}
}