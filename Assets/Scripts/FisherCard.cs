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
	public FisherImageGrabber fisherImageGrabber;
	
	[Space]
	public PriceButton wormsTankPriceButton;
	public PriceButton fishingRodPriceButton;
	public PriceButton umbrellaPriceButton;

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

		fisherImageGrabber.fisher = target;

		target.ready = true;
		target.ViewUpdated.AddListener(UpdateView);
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
		fisherName.text = $"Fisher #{id + 1} (Level: {fisher.level + 1})";
		wormsTank.text = $"Worms tank: {fisher.MaxWormsAmount}";
		rod.text = $"Fishing rod: {fisher.FPS} FPS";

		if (fisher.wormsTankLevel == fisher.wormsTankUpgrades.Length - 1)
		{
			wormsTankPriceButton.SoldOut();
		}
		else
		{
			wormsTankPriceButton.price = fisher.wormsTankUpgrades[fisher.wormsTankLevel + 1].price;
			wormsTankPriceButton.UpdatePrice();
		}
		
		if (fisher.fishingRodLevel == fisher.rodsUpgrades.Length - 1)
		{
			fishingRodPriceButton.SoldOut();
		}
		else
		{
			fishingRodPriceButton.price = fisher.rodsUpgrades[fisher.fishingRodLevel + 1].price;
			fishingRodPriceButton.UpdatePrice();
		}

		if (fisher.hasUmbrella)
		{
			umbrellaPriceButton.SoldOut();
		}
		else
		{
			umbrellaPriceButton.price = fisher.umbrellaPrice;
			umbrellaPriceButton.UpdatePrice();
		}
	}

	public void BuyFishingRod()
	{
		fisher.fishingRodLevel++;
		fisher.UpdateView();
	}

	public void BuyWormsTank()
	{
		fisher.wormsTankLevel++;
		fisher.UpdateView();
	}

	public void BuyUmbrella()
	{
		fisher.hasUmbrella = true;
		fisher.UpdateView();
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