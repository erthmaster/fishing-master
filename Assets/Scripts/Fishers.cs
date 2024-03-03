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
	}
}