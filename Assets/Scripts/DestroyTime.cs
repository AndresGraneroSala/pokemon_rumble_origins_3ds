using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTime : MonoBehaviour {

	[SerializeField] private float destroyTime = 3f;
	private float _timer = 0f;
	
	public void Config(float moveDistance, float moveSpeed)
	{
		_timer = 0f;
		float movement = moveDistance / moveSpeed;
		destroyTime = movement;
		destroyTime += 0.05f;
	}

	// Update is called once per frame
	void Update () {
		_timer+=Time.deltaTime;
		if (_timer >= destroyTime)
		{
			//gameObject.SetActive(false);
		}
	}
}
