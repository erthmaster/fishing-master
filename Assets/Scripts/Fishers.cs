using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fishers : MonoBehaviour
{
	public static Fishers Instance;

	public Fisher[] FishersList;
	
	private void Awake()
	{
		Instance ??= this;

		for (var i = 0; i < FishersList.Length; i++)
		{
			Fisher fisher = FishersList[i];
			fisher.fisherName.text = $"Fisher #{i + 1}";
		}
		
		InvokeRepeating(nameof(UpdateFPS), 0, 0.5f);
	}

	public void UpdateFPS()
	{
		Player.Instance.FishPerSecond = FishersList.Where(fisher => fisher.gameObject.activeInHierarchy && fisher.state == FisherState.Working).Sum(fisher => fisher.FPS);
	}
}