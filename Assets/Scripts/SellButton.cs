using System;
using AurumGames.Animation;
using AurumGames.Animation.Tracks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SellButton : MonoBehaviour
{
	public TextMeshProUGUI priceText;
	public Image button;
	public Color normal;
	public Color soldColor;
	public string soldText;

	public int fishPerCoin = 10;

	private void Start()
	{
		Player.Instance.FishAmountChanged.AddListener(() =>
		{
			UpdateState();
		});
		
		
		UpdateState();
	}

	public void UpdateState()
	{
		var price = GetPrice();
		if (price > 0)
		{
			button.raycastTarget = true;
			button.color = normal;
			priceText.text = $"+{price}<sprite=0>";
		}
		else
		{
			button.raycastTarget = false;
			button.color = soldColor;
			priceText.text = soldText;
		}
	}

	private int GetPrice()
	{
		return Player.Instance.FishAmount / fishPerCoin;
	}

	public void Sell()
	{
		var price = GetPrice();
		Currency.Instance.Money += price;
		Player.Instance.FishAmount -= price * fishPerCoin;
		
		UpdateState();
	}
}