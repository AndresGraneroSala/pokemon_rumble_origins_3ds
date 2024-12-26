using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{

	private float _damage=0;
	[HideInInspector] public bool isPlayer=true;

	private List<GameObject> enemiesAttacked;

	private void Start()
	{
		enemiesAttacked = new List<GameObject>();
	}

	public void SetDamage(float damage)
	{
		_damage = damage;
	}
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Enemy") && isPlayer)
		{
			if (!enemiesAttacked.Contains(other.gameObject))
			{
				enemiesAttacked.Add(other.gameObject);
				other.GetComponent<LifeDestroy>().Damage(_damage);
			}
		}
		
		if (other.CompareTag("Player") && isPlayer)
		{
			return;
		}

		
	}
}
