using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{

	private float _damage = 0;
	private bool _isPlayer = true;

	private List<GameObject> enemiesAttacked;

	private Attack.TypeAttack _typeAttack;

	private void Start()
	{
		enemiesAttacked = new List<GameObject>();
	}

	public void SetDamage(float damage, Attack.TypeAttack type, bool isPlayer)
	{
		enemiesAttacked = new List<GameObject>();
		_isPlayer = isPlayer;
		_typeAttack = type;

		_damage = damage;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Enemy") && _isPlayer)
		{
			if (!enemiesAttacked.Contains(other.gameObject))
			{
				Opponent opponent = other.GetComponent<Opponent>();
				float multiplier = Attack.GetMultiplicatorType(_typeAttack, opponent.TypePokemon1);


				if (opponent.TypePokemon2 != Attack.TypeAttack.None)
				{
					multiplier *= Attack.GetMultiplicatorType(_typeAttack, opponent.TypePokemon2);
				}




				enemiesAttacked.Add(other.gameObject);
				other.GetComponent<LifeDestroy>().Damage(_damage, multiplier);
			}
		}

		if (other.CompareTag("Player") && !_isPlayer)
		{
			if (!enemiesAttacked.Contains(other.gameObject))
			{
				enemiesAttacked.Add(other.gameObject);

				PlayerStats stats = other.GetComponent<PlayerStats>();
				float multiplier = Attack.GetMultiplicatorType(_typeAttack, stats.Type1);


				if (stats.Type2 != Attack.TypeAttack.None)
				{
					multiplier *= Attack.GetMultiplicatorType(_typeAttack, stats.Type2);
				}
				other.GetComponent<LifeDestroy>().Damage(_damage, multiplier);
			}
		}
	}
}
