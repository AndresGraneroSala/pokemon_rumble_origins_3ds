using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Coin : MonoBehaviour {
	private string[] tagsToDestroy = new []{"Player"};
	private float timeToClaim=1f;
	private float timer=0f;
	private void Start()
	{
		timeToClaim = CoinManager.instance.TimeToClaimCoins;
	}

	private void Update()
	{
		timer += Time.deltaTime;

	}

	private void OnTriggerEnter(Collider other)
	{
		if (timer<timeToClaim)
		{
			return;
		}
		
		foreach (string tagDesrtoy in tagsToDestroy)
		{
			if (other.CompareTag(tagDesrtoy))
			{
				CoinManager.instance.ResetTimer();
				Destroy(gameObject);
				break;
			}
		}
	}
}
