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
			var circleCollider2D = GetComponent<CircleCollider2D>();
			Vector2 randomCircle = Random.insideUnitCircle * circleCollider2D.radius + circleCollider2D.offset;
			Vector3 spawnPosition = new Vector3(randomCircle.x, randomCircle.y, 0) + transform.position;
			
			GameObject wormBox = Instantiate(box, spawnPosition, Quaternion.identity);
			wormBox.GetComponent<WormBox>().amount = Random.Range(5, 30);
			nextBox = Random.Range(5, 20);
		}
	}
}
