using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup), typeof(Image))]
public class Button : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler
{
	public UnityEvent Click;
	
	public Sprite normal;
	public Sprite pressed;

	public bool Interactable
	{
		get => _interactable;
		set
		{
			if (_image == null)
				_image = GetComponent<Image>();

			if (_canvasGroup == null)
				_canvasGroup = GetComponent<CanvasGroup>();

			_interactable = value;

			_image.sprite = normal;
			_canvasGroup.alpha = value ? 1 : 0.7f;
		}
	}

	[SerializeField] private bool _interactable = true;
	private CanvasGroup _canvasGroup;
	private Image _image;

	private void Awake()
	{
		_image = GetComponent<Image>();
		_canvasGroup = GetComponent<CanvasGroup>();

		Interactable = _interactable;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (Interactable == false)
			return;

		_image.sprite = pressed;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (Interactable == false)
			return;
		
		Click.Invoke();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		_image.sprite = normal;
	}
}