using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeDestroy : MonoBehaviour
{

	[SerializeField] private float life=100;


	public void Damage(float damage)
	{
		life -= damage;
	}
	
	// Update is called once per frame
	void Update () {
		if (life<=0)
		{
			Destroy(gameObject);
		}
	}
	
	//TODO: barra de vida
}
