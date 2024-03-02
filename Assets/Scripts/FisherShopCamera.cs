using System;
using Cinemachine;
using UnityEngine;

public class FisherShopCamera : MonoBehaviour
{
	public CinemachineVirtualCamera active;
	public CinemachineVirtualCamera virtualCamera1;
	public CinemachineVirtualCamera virtualCamera2;

	private void Awake()
	{
		virtualCamera2 = Instantiate(virtualCamera1, virtualCamera1.transform.parent);
		virtualCamera2.gameObject.SetActive(false);
		active = virtualCamera1;
		virtualCamera1.gameObject.SetActive(true);
		
		gameObject.SetActive(false);
	}

	public void Deactivate()
	{
		gameObject.SetActive(false);
	}

	public void Show(Fisher target)
	{
		Transform targetTransform = target.transform;
		active.gameObject.SetActive(false);
		active = active == virtualCamera1 ? virtualCamera2 : virtualCamera1;
		
		active.Follow = targetTransform;
		if (gameObject.activeInHierarchy == false)
			active.transform.position = targetTransform.position;
		active.gameObject.SetActive(true);
		
		gameObject.SetActive(true);
	}
}