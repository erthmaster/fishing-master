using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WormPost : MonoBehaviour, IUpdatable
{
	public static WormPost Instance;
	public TextMeshPro title;
	public int nextBox;
	public GameObject box;

	private void Awake()
	{
		Instance ??= this;
	}
	
	private void Start()
	{
		Updater.Instance.Add(this);
	}

	public void GameUpdate()
	{
	}

	public void GameTick()
	{
		nextBox--;
		if (nextBox <= 0)
		{
			Vector2 randomCircle = Random.insideUnitCircle * GetComponent<CircleCollider2D>().radius;
			Vector3 spawnPosition = new Vector3(randomCircle.x, randomCircle.y, 0) + transform.position;
			
			GameObject wormBox = Instantiate(box, spawnPosition, Quaternion.identity);
			wormBox.GetComponent<WormBox>().amount = Random.Range(5, 15);
			nextBox = Random.Range(3, 10);
		}
	}
}
