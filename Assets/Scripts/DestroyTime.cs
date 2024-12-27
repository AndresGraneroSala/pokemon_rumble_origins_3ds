using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTime : MonoBehaviour {

	[SerializeField] private float destroyTime = 3f;
	private float _timer = 0f;
	
	// Update is called once per frame
	void Update () {
		_timer+=Time.deltaTime;
		if (_timer >= destroyTime)
		{
			Destroy(gameObject);
		}
	}
}
