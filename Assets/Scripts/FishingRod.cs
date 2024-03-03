using UnityEngine;
using UnityEngine.Serialization;

public class FishingRod : MonoBehaviour
{
	public Sprite[] fishingRods;
	public SpriteRenderer renderer;

	public void Show(int level)
	{
		renderer.sprite = fishingRods[level];
		gameObject.SetActive(true);
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}
}