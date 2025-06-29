﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CatchPokemon : MonoBehaviour
{

	[SerializeField] private MonoBehaviour[] enemyScripts;
	[SerializeField] private MonoBehaviour[] playerScripts;
	
	[Range(0f, 1f)]
	[SerializeField] private float accuracy;

	private void Awake()
	{
		for (int i = 0; i < enemyScripts.Length; i++)
		{
			enemyScripts[i].enabled = true;
		}
		
		
		for (int i = 0; i < playerScripts.Length; i++)
		{
			playerScripts[i].enabled = false;
		}
	}

	public void ChangeToPlayer()
	{
		float random = Random.value;
		
		if (random > accuracy)
		{
			Destroy(gameObject);
			return;
		}
		
		GameObject player = GameObject.FindGameObjectWithTag("Player");

		for (int i = 0; i < enemyScripts.Length; i++)
		{
			Destroy( enemyScripts[i]);
		}
		
		for (int i = 0; i < playerScripts.Length; i++)
		{
			playerScripts[i].enabled = true;
		}
		
		gameObject.transform.SetParent(player.transform.parent);
		gameObject.transform.localPosition = player.transform.localPosition;
		
		gameObject.tag = "Player";
		gameObject.layer = LayerMask.NameToLayer("Player");
		Rigidbody rb = gameObject.AddComponent<Rigidbody>();
		rb.useGravity = false;
		rb.isKinematic = true;
		rb.constraints = RigidbodyConstraints.FreezeAll;

		ChangePokemon.instance.EnqueuePokemon(gameObject);
		
		gameObject.SetActive(false);
	}
}
