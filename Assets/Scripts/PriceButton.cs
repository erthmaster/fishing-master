using System;
using AurumGames.Animation;
using AurumGames.Animation.Tracks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PriceButton : MonoBehaviour
{
	public UnityEvent Bought;
	
	public int price;

	public TextMeshProUGUI priceText;
	public Image button;
	public Image canBuyIcon;
	public Color normal;
	public Color canBuy;
	public Color soldColor;
	public string soldText;
	
	private AnimationPlayer _animation;
	private bool _soldOut;
	
	private void Awake()
	{
		var self = priceText.transform as RectTransform;

		Vector2 anchoredPosition = self.anchoredPosition;
		_animation = new AnimationPlayer(this, new ITrack[]
		{
			new AnchoredPositionTrack(self, new []
			{
				new KeyFrame<Vector2>(0, anchoredPosition + new Vector2(10, 0), Easing.QuadOut),
				new KeyFrame<Vector2>(100, anchoredPosition - new Vector2(10, 0), Easing.QuadOut),
				new KeyFrame<Vector2>(200, anchoredPosition + new Vector2(10, 0), Easing.QuadOut),
				new KeyFrame<Vector2>(300, anchoredPosition - new Vector2(10, 0), Easing.QuadOut),
				new KeyFrame<Vector2>(400, anchoredPosition, Easing.QuadOut)
			}),
			new ScaleTrack(self, new []
			{
				new KeyFrame<Vector3>(0, Vector3.one, Easing.QuadOut),
				new KeyFrame<Vector3>(100, Vector3.one * 1.15f, Easing.QuadOut),
				new KeyFrame<Vector3>(400, Vector3.one * 1.15f, Easing.QuadOut),
				new KeyFrame<Vector3>(500, Vector3.one, Easing.QuadOut)
			}),
			new TextMeshProColorTrack(priceText, new []
			{
				new KeyFrame<Color>(0, priceText.color, Easing.QuadOut),
				new KeyFrame<Color>(100, Color.red, Easing.QuadOut),
				new KeyFrame<Color>(400, Color.red, Easing.QuadOut),
				new KeyFrame<Color>(500, priceText.color, Easing.QuadOut),
			})
		});
	}

	private void Start()
	{
		Currency.Instance.MoneyValueChanged.AddListener(() =>
		{
			if (_soldOut)
			{
				SoldOut();
			}
			else
			{
				UpdatePrice();
			}
		});
		
		
		UpdatePrice();
	}

	public void UpdatePrice()
	{
		priceText.text = $"{price}<sprite=0>";

		UpdateState();
	}

	public void UpdateState()
	{
		_soldOut = false;
		if (button)
		{
			button.raycastTarget = true;
			var can = Currency.Instance.CanBuy(price);
			button.color = can ? canBuy : normal;
			if (canBuyIcon)
			{
				canBuyIcon.gameObject.SetActive(can);
			}
		}
	}

	public void SoldOut()
	{
		_soldOut = true;
		if (button)
		{
			button.raycastTarget = false;
			button.color = soldColor;
			if (canBuyIcon)
			{
				canBuyIcon.gameObject.SetActive(false);
			}
		}

		priceText.text = soldText;
	}

	public void Buy()
	{
		if (Currency.Instance.CanBuy(price) == false)
		{
			_animation.PlayFromStart();
			return;
		}
		
		Currency.Instance.Buy(price);
		
		Bought.Invoke();
	}
}