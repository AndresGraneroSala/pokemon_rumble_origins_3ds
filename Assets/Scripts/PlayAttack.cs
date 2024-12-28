using System;
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

	private void Start()
	{
		_playerMove = GetComponent<PlayerMove>();
		_opponent = GetComponent<Opponent>();
		_playerAttack = GetComponent<PlayerAttack>();
		isPlayer = _playerAttack;
		
		
		target = GameObject.FindGameObjectWithTag("Player").transform;

		
	}

	public IEnumerator Play(Attack attack)
	{


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

		yield return new WaitForSeconds(attack.Delay);

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

		if (attack.Bullet != null)
		{
			GameObject bullet = Instantiate(attack.Bullet, spawnBullets);


			AttackCollision[] damages = bullet.GetComponentsInChildren<AttackCollision>();
			foreach (var damage in damages)
			{

				damage.SetDamage(attack.Damage, attack.Type, isPlayer);
			}

			if (bullet.GetComponent<Billboard>())
			{
				bullet.GetComponent<Billboard>().SetBillboard(model.eulerAngles.y);
			}

			if (attack.MovePlayer>0)
			{
				bullet.GetComponent<DestroyTime>().Config(attack.MovePlayer, attack.MovePlayerSpeed);
			}
		}

		if (!isPlayer)
		{
			_opponent.ChangeSpeedBones(1);
			_opponent.IsAttacking = false;
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
				if (_opponent.IsColliderInFront())
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
		while (true)
		{
			// Calcula la dirección hacia el objetivo
			Vector3 direction = target.position - transform.position;

			// Si la dirección es muy pequeña, termina la rotación
			if (direction.magnitude < 0.01f)
			{
				yield break;
			}

			// Calcula la rotación deseada
			Quaternion targetRotation = Quaternion.LookRotation(direction);

			// Aplica una rotación suave hacia la rotación deseada
			transform.rotation = Quaternion.Lerp(
				transform.rotation,
				targetRotation,
				_opponent.RotationSpeedDelay * Time.deltaTime
			);

			// Espera al siguiente frame antes de continuar
			yield return null;
		}
	}
}
