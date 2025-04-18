﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayAttack : MonoBehaviour {

	private bool isPlayer=false;
	
	[SerializeField] RotateBone rotateBone;

	[SerializeField] private Transform model;

	[SerializeField] private Transform spawnBullets;

	private PlayerMove _playerMove;

	private PlayerAttack _playerAttack;
	
	private Opponent _opponent;
	
	private Transform target;

	private GameObject _prefAttack1, _prefAttack2;
	
	private void Start()
	{
		_playerMove = GetComponent<PlayerMove>();
		_opponent = GetComponent<Opponent>();
		_playerAttack = GetComponent<PlayerAttack>();
		isPlayer = gameObject.CompareTag("Player");
		
		
		target = GameObject.FindGameObjectWithTag("Player").transform;

		
	}

	public void InitPool(Attack attack,int posAtt=1)
	{
		if (!attack)
		{
			return;
		}
		
		if (attack.Bullet==null)
		{
			return;
		}
		if (posAtt==1)
		{
			_prefAttack1 = Instantiate(attack.Bullet, spawnBullets);
			_prefAttack1.SetActive(false);
		}
		else
		{
			_prefAttack2 = Instantiate(attack.Bullet, spawnBullets);
			_prefAttack2.SetActive(false);
		}
	}

	public IEnumerator Play(Attack attack, int posAtt=1)
	{
		if (!attack)
		{
			yield break;
		}

		if (isPlayer&& GameManager.instance.PlayerSpeed<=0)
		{
			yield break;
		}


		if (isPlayer)
		{
			_playerMove.block = true;
			_playerAttack.Timer = attack.FireRate;
		}
		else
		{
			_opponent.IsAttacking = true;
			_opponent.ChangeSpeedBones(0.25f);
		}


		rotateBone.Configure(attack.Rotate);

		if (!isPlayer)
		{
			StartCoroutine(RotateTowardsCoroutine());
		}

		if (isPlayer)
		{
			yield return new WaitForSeconds(attack.Delay);
		}
		else
		{
			yield return new WaitForSeconds(attack.DelayOpponent);
		}

		if (!isPlayer)
		{
			StopCoroutine(RotateTowardsCoroutine());
		}

		rotateBone.PlayAttack();

		if (attack.MovePlayer > 0)
		{
			if (!attack.IsBulletBefore)
			{
				yield return StartCoroutine(MoveCoroutine(attack.MovePlayer, attack.MovePlayerSpeed));

			}
			else
			{
				StartCoroutine(MoveCoroutine(attack.MovePlayer, attack.MovePlayerSpeed));

			}
		}

		
		if (!isPlayer)
		{
			_opponent.IsAttacking = false;
			_opponent.ChangeSpeedBones(1);
		}
		
		
		
		if (attack.Bullet != null)
		{
			GameObject bullet = posAtt == 1 ? _prefAttack1 : _prefAttack2;
			bullet.SetActive(true);
			
			bullet.GetComponent<AttackCollision>().SetDamage(attack.Damage, attack.Type, isPlayer);
			
			if (bullet.GetComponent<Billboard>())
			{
				bullet.GetComponent<Billboard>().SetBillboard(model.eulerAngles.y);
			}

			if (bullet.GetComponent<DestroyTime>())
			{
				bullet.GetComponent<DestroyTime>().Config(attack.MovePlayer, attack.MovePlayerSpeed);
			}
		}
	}

	private IEnumerator MoveCoroutine(float distance, float speed)
	{
		if (isPlayer)
		{
			_playerMove.block = true;
		}

		Vector3 startPosition = transform.position; // Posición inicial del objeto
		Vector3 targetPosition = startPosition + model.forward * distance; // Posición objetivo

		float elapsedTime = 0f;
		float journeyLength = Vector3.Distance(startPosition, targetPosition);

		while (elapsedTime < journeyLength / speed)
		{
			if (isPlayer)
			{
				if (_playerMove.IsColliderInFront())
				{
					yield break;
				}
			}
			else
			{
				if (_opponent.IsColliderInAnyDirection())
				{
					yield break;
				}
			}
			
			// Interpolar la posición
			transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime * speed) / journeyLength);
			elapsedTime += Time.deltaTime;
			yield return null; // Espera un frame
		}

		// Asegurarse de llegar a la posición final
		transform.position = targetPosition;

		if (isPlayer)
		{
			_playerMove.block = false;
		}
	}
	
	private IEnumerator RotateTowardsCoroutine()
	{
		Transform rotateTarget = _opponent.Model; // Usa el modelo de rotación

		while (true)
		{
			Vector3 direction = target.position - transform.position;

			if (direction.magnitude < 0.01f)
			{
				yield break;
			}

			Quaternion targetRotation = Quaternion.LookRotation(direction);

			rotateTarget.rotation = Quaternion.Lerp(
				rotateTarget.rotation,
				targetRotation,
				_opponent.RotationSpeedDelay * Time.deltaTime
			);

			yield return null;
		}
	}
}
