using TMPro;
using UnityEngine;

public class WormsBucket : MonoBehaviour, IInteractable
{
	public Fisher fisher;
	public Sprite withWorms;
	public Sprite withoutWorms;

	public SpriteRenderer renderer;
	public TextMeshProUGUI wormsAmount;
	public Color NormalTextColor;
	public NoWormsPanel noWorms;

	public InfoPanel infoPanel;

	public void UpdateView()
	{
		wormsAmount.text = $"{fisher.WormsAmount}/{fisher.MaxWormsAmount}";
		var hasWorms = fisher.WormsAmount > 0;
		wormsAmount.color = hasWorms ? NormalTextColor : Color.red;
		renderer.sprite = hasWorms ? withWorms : withoutWorms;

		if (hasWorms)
		{
			noWorms.Hide();
		}
		else
		{
			noWorms.Show();
		}
	}
	
	public void Interact()
	{
		fisher.GiveWorms();
	}

	public void BecomeInteractTarget()
	{
		infoPanel.BecomeInteractTarget(this);
	}

	public void StopBeingInteractTarget()
	{
		infoPanel.StopBeingInteractTarget();
	}
}