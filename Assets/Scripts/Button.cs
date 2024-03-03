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

	public RectTransform target;

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
	private Vector2 _position;

	private void Awake()
	{
		_image = GetComponent<Image>();
		_canvasGroup = GetComponent<CanvasGroup>();

		Interactable = _interactable;
		_position = target.anchoredPosition;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (Interactable == false)
			return;

		_image.sprite = pressed;
		target.anchoredPosition = _position - new Vector2(0, 10 / _image.pixelsPerUnitMultiplier);
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
		target.anchoredPosition = _position;
	}
}