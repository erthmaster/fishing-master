using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fishers : MonoBehaviour
{
	public static Fishers Instance;

	public readonly List<Fisher> FishersList = new();
	
	private void Awake()
	{
		Instance ??= this;
	}

	public void Add(Fisher fisher)
	{
		FishersList.Add(fisher);
	}
}