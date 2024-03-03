using System;
using AurumGames.Animation;
using AurumGames.Animation.Tracks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Currency : MonoBehaviour
{
	public static Currency Instance;

	public UnityEvent MoneyValueChanged;

	public TextMeshProUGUI text;

	public int Money
	{
		get => _money;
		set
		{
			_money = value;
			MoneyValueChanged.Invoke();

			if (_animation != null)
			{
				_track.Set(value);
				_animation.Update();
			}
		}
	}

	[SerializeField] private int _money;

	private FluentFloatTrack _track;
	private FluentAnimationPlayer _animation;

	private void Awake()
	{
		Instance ??= this;
		
		_track = new FluentFloatTrack(_money, value =>
		{
			text.text = value.ToString("F0") + " <sprite=0>";
		}, new Transition(300, Easing.QuadOut));

		_animation = new FluentAnimationPlayer(this, _track);
		Money = _money;
	}

	public bool CanBuy(int price)
	{
		return Money >= price;
	}

	public void Buy(int price)
	{
		if (CanBuy(price) == false)
			throw new Exception("Can`t buy not enough money");

		Money -= price;
	}
}