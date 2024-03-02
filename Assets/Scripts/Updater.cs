using System;
using System.Collections.Generic;
using UnityEngine;

public class Updater : MonoBehaviour
{
	public static Updater Instance;

	public float tickTime;
	
	private readonly List<IUpdatable> _updatables = new();
	private float _time;

	private void Awake()
	{
		Instance ??= this;
	}

	private void Update()
	{
		if (GameManager.Instance.isPlaying == false)
			return;

		_time += Time.deltaTime;
		
		foreach (IUpdatable updatable in _updatables)
		{
			updatable.GameUpdate();
			if (_time >= tickTime)
				updatable.GameTick();
		}

		if (_time >= tickTime)
			_time = 0;
	}

	public void Add(IUpdatable updatable)
	{
		_updatables.Add(updatable);
	}
}