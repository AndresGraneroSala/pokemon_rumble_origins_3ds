using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lifebar : MonoBehaviour {

	[SerializeField] private Transform green;

	public void ChangeLife(float lifeSize)
	{
		green.localScale = new Vector3(lifeSize, green.localScale.y, green.localScale.z);
		/*if (lifeSize<=0)
		{
			Destroy(gameObject);
		}*/
	}
}
