using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Opponent : MonoBehaviour {

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform target;
    
    [SerializeField] private float distanceToAttack=1;
    [SerializeField] private float distanceToMove=10;
    
    [SerializeField] private Attack attack;
	[SerializeField] private Transform spawnBullets;
    
    private RotateBone [] _bones;
    private RotateBone _boneAttack;
    
    [SerializeField] float rotationSpeedDelay=10;
    
    private float _distanceToPlayer=0;

    private bool _isAttacking;
	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag("Player").transform;
		agent.stoppingDistance = distanceToAttack;
		
		_bones = GetComponentsInChildren<RotateBone>()
			.Where(bone => !bone.IsAttack) // Filtra los que no son isAttack
			.ToArray();
		
		_boneAttack = GetComponentsInChildren<RotateBone>()
			.Where(bone => bone.IsAttack) // Filtra los que no son isAttack
			.ToArray()[0];	}
	
	// Update is called once per frame
	void Update()
	{

		if (_isAttacking)
		{
			return;
		}

		if (agent.pathPending || agent.remainingDistance == Mathf.Infinity || agent.remainingDistance <= 0.0f)
		{
			_distanceToPlayer = Vector3.Distance(transform.position, target.position);
			agent.isStopped = true;
			ChangeSpeedBones(0);

		}
		else
		{
			agent.isStopped = false;

			// Muestra la distancia restante en la consola.
			_distanceToPlayer = agent.remainingDistance;
		}

		agent.SetDestination(target.position);

		if (_distanceToPlayer <= distanceToMove)
		{
			if (_distanceToPlayer <= distanceToAttack)
			{
				StartCoroutine(PlayAttack(attack));
			}
		}
	}

	private void ChangeSpeedBones(float speed)
	{
		foreach (var bone in _bones)
		{
			bone.SetSpeedState(speed);
		}
	}
	
	private IEnumerator PlayAttack(Attack attack)
	{
		
		_isAttacking = true;
		_boneAttack.Configure(attack.Rotate);
		ChangeSpeedBones(0.25f);

		StartCoroutine( RotateTowardsCoroutine());
		
		yield return new WaitForSeconds(attack.Delay);
		
		StopCoroutine(RotateTowardsCoroutine());
		
		_boneAttack.PlayAttack();

		

		if (attack.Bullet!=null)
		{
			GameObject bullet= Instantiate(attack.Bullet, spawnBullets.position, Quaternion.identity,spawnBullets);
			AttackCollision [] damages= bullet.GetComponentsInChildren<AttackCollision>();
			foreach (var damage in damages)
			{
				damage.SetDamage(attack.Damage);
				damage.isPlayer = false;
			}
			
			
			bullet.GetComponent<Billboard>().SetBillboard(transform.eulerAngles.y);
		}
		
		if (attack.MovePlayer>0)
		{
			yield return StartCoroutine(MoveCoroutine(attack.MovePlayer,attack.MovePlayerSpeed));
		}
		_isAttacking = false;
		ChangeSpeedBones(1);

	}
	
	private IEnumerator MoveCoroutine(float distance, float speed)
	{
		//todo: check collision

		Vector3 startPosition = transform.position; // Posición inicial del objeto
		Vector3 targetPosition = startPosition + transform.forward * distance; // Posición objetivo

		float elapsedTime = 0f;
		float journeyLength = Vector3.Distance(startPosition, targetPosition);

		while (elapsedTime < journeyLength / speed)
		{
			
			// Interpolar la posición
			transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime * speed) / journeyLength);
			elapsedTime += Time.deltaTime;
			yield return null; // Espera un frame
		}

		// Asegurarse de llegar a la posición final
		transform.position = targetPosition;
		
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
				rotationSpeedDelay * Time.deltaTime
			);

			// Espera al siguiente frame antes de continuar
			yield return null;
		}
	}
	
}
