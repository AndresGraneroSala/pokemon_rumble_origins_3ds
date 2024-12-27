using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
	[SerializeField] RotateBone rotateBone;

	[SerializeField] private Attack attack1;
	[SerializeField] private Attack attack2;
	private bool isAttacking = false;
	[SerializeField] private Transform model;
	private float timer = 0f;

	[SerializeField] private Transform spawnBullets;

	private PlayerMove _playerMove;
	// Use this for initialization
	void Start () {
		_playerMove = gameObject.GetComponent<PlayerMove>();
	}
	
	// Update is called once per frame
	void Update()
	{
		if (isAttacking)
		{
			timer -= Time.deltaTime;
			if (timer<=0)
			{
				isAttacking = false;
				_playerMove.block = false;
			}
		}
		
		if (!isAttacking && (Input.GetKeyDown(KeyCode.Mouse0) || UnityEngine.N3DS.GamePad.GetButtonTrigger(N3dsButton.A)))
		{
			isAttacking = true;
			StartCoroutine( PlayAttack(attack1));
		}
		
		if (!isAttacking && (Input.GetKey(KeyCode.Mouse1) || UnityEngine.N3DS.GamePad.GetButtonTrigger(N3dsButton.B)))
		{
			StartCoroutine( PlayAttack(attack2));
		}

		

	}

	private IEnumerator PlayAttack(Attack attack)
	{
		_playerMove.block = true;
		isAttacking = true;
		timer = attack.FireRate;

		
		rotateBone.Configure(attack.Rotate);

		yield return new WaitForSeconds(attack.Delay);
		
		rotateBone.PlayAttack();

		if (attack.MovePlayer>0)
		{
			yield return StartCoroutine(MoveCoroutine(attack.MovePlayer,attack.MovePlayerSpeed));
		}

		if (attack.Bullet!=null)
		{
			GameObject bullet= Instantiate(attack.Bullet, spawnBullets.position, Quaternion.identity,spawnBullets);
			AttackCollision [] damages= bullet.GetComponentsInChildren<AttackCollision>();
			foreach (var damage in damages)
			{
				
				damage.SetDamage(attack.Damage, attack.Type,true);
			}
			
			
			bullet.GetComponent<Billboard>().SetBillboard(model.eulerAngles.y);
		}
	}
	
	private IEnumerator MoveCoroutine(float distance, float speed)
	{
		
		_playerMove.block = true;

		Vector3 startPosition = transform.position; // Posición inicial del objeto
		Vector3 targetPosition = startPosition + model.forward * distance; // Posición objetivo

		float elapsedTime = 0f;
		float journeyLength = Vector3.Distance(startPosition, targetPosition);

		while (elapsedTime < journeyLength / speed)
		{
			if (_playerMove.IsColliderInFront())
			{
				yield break;
			}
			// Interpolar la posición
			transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime * speed) / journeyLength);
			elapsedTime += Time.deltaTime;
			yield return null; // Espera un frame
		}

		// Asegurarse de llegar a la posición final
		transform.position = targetPosition;
		
		_playerMove.block = false;
	}
}
