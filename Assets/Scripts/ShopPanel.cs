using System;
using System.Collections.Generic;
using System.Linq;
using AurumGames.Animation;
using AurumGames.Animation.Tracks;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
	public FisherCard prefab;
	public Transform container;
	public PriceButton buyNewFisherButton;
	
	public FisherShopCamera shopCamera;
	public FisherCard selected;

	private readonly List<FisherCard> _cards = new();
	private StatedAnimationPlayer<Visibility> _window;

	private void Awake()
	{
		var canvasGroup = GetComponent<CanvasGroup>();
		var rectTransform = transform as RectTransform;
		
		var show = new TracksEvaluator(new ITrack[]
		{
			new CanvasGroupOpacityTrack(canvasGroup, FloatTrack.KeyFrames01(new TransitionStruct(300, Easing.QuadOut)))
		});

		var hide = new TracksEvaluator(new ITrack[]
		{
			new CanvasGroupOpacityTrack(canvasGroup, FloatTrack.KeyFrames10(new TransitionStruct(300, Easing.QuadOut)))
		});

		_window = new StatedAnimationPlayer<Visibility>(this, new Dictionary<Visibility, TracksEvaluator>()
		{
			{ Visibility.Visible, show },
			{ Visibility.Hidden, hide }
		});
		_window.StateChanged += (previous, current) =>
		{
			switch (current)
			{
				case Visibility.Visible:
					canvasGroup.blocksRaycasts = true;
					canvasGroup.interactable = true;

					if (selected != null)
					{
						shopCamera.Show(selected.fisher);
						selected.fisher.BecomeInteractTarget();
					}

					if (Player.Instance != null)
						Player.Instance.blockMovement = true;
					break;
				
				case Visibility.Hidden:
					canvasGroup.blocksRaycasts = false;
					canvasGroup.interactable = false;
					
					shopCamera.Deactivate();
					if (selected != null)
					{
						selected.fisher.StopBeingInteractTarget();
					}
					
					if (Player.Instance != null)
						Player.Instance.blockMovement = false;
					break;
			}
		};
		
		_window.SetStateInstant(Visibility.Hidden);
	}
	
	public void Interact()
	{
		if (_window.IsPlaying && _window.Percent is <= 0.85f and >= 0.25f)
			return;

		_window.SetState(_window.CurrentState == Visibility.Visible ? Visibility.Hidden : Visibility.Visible);
	}
	
	public void Close()
	{
		if (_window.IsPlaying && _window.Percent is <= 0.85f and >= 0.25f)
			return;
		
		_window.SetState(Visibility.Hidden);	
	}

	public void SetSelectedItem(FisherCard selectedItem)
	{
		if (selected == selectedItem)
			return;

		if (selected != null)
		{
			selected.Unselect();
			selected.fisher.StopBeingInteractTarget();
		}

		selected = selectedItem;
		selected.fisher.BecomeInteractTarget();
		shopCamera.Show(selected.fisher);
	}

	public void BuyNew()
	{
		CreateNewFisher();

		if (_cards.Count == Fishers.Instance.FishersList.Count)
		{
			buyNewFisherButton.SoldOut();
		}
		else
		{
			buyNewFisherButton.UpdatePrice();
		}
	}

	private void CreateNewFisher()
	{
		FisherCard newFisher = Instantiate(prefab, container);
		newFisher.Bind(Fishers.Instance.FishersList[_cards.Count], _cards.Count, this);
		newFisher.gameObject.SetActive(true);
		_cards.Add(newFisher);
		newFisher.Select();
	}
}