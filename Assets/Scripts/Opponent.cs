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

    public float RotationSpeedDelay
    {
	    get { return rotationSpeedDelay; }
    }

    private float _distanceToPlayer=0;

    private bool _isAttacking;

    public bool IsAttacking
    {
	    get { return _isAttacking; }
	    set { _isAttacking = value; }
    }

    private float _upChecker = 0.1f;

    [SerializeField] private Attack.TypeAttack typePokemon1;

    public Attack.TypeAttack TypePokemon1
    {
	    get { return typePokemon1; }
    }
    [SerializeField] private Attack.TypeAttack typePokemon2;

    private PlayAttack _playAttack;
    public Attack.TypeAttack TypePokemon2
    {
	    get { return typePokemon2; }
    }

    // Use this for initialization
	void Start ()
	{

		_playAttack = GetComponent<PlayAttack>();
		
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

		if (agent.pathPending || agent.remainingDistance == Mathf.Infinity || agent.remainingDistance <= 0.0f|| agent.isStopped)
		{
			_distanceToPlayer = Vector3.Distance(transform.position, target.position);

			if (!agent.isStopped)
			{
				ChangeSpeedBones(0);
			}
			
			agent.isStopped = true;

		}
		else
		{
			// Muestra la distancia restante en la consola.
			_distanceToPlayer = agent.remainingDistance;
		}

		if (_distanceToPlayer <= distanceToMove) 
		{
			if (_distanceToPlayer <= distanceToAttack) 
			{
				// Detenemos al agente cuando está dentro del rango de ataque
				agent.isStopped = true;
        
				// Ejecutamos la animación o lógica de ataque
				//StartCoroutine(PlayAttack(attack));
				StartCoroutine(_playAttack.Play(attack));
			}
			else
			{
				// Si está dentro del rango de persecución pero no de ataque, el agente sigue persiguiendo
				agent.isStopped = false;
				agent.SetDestination(target.position);
			}
		}
		else
		{
			// Si el jugador se aleja del rango de persecución, detenemos al agente
			agent.isStopped = true;
		}
	}

	public void ChangeSpeedBones(float speed)
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
				damage.SetDamage(attack.Damage, attack.Type,false);
				
			}

			if (bullet.GetComponent<Billboard>())
			{
				bullet.GetComponent<Billboard>().SetBillboard(transform.eulerAngles.y);
			}
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
		Vector3 startPosition = transform.position; // Posición inicial del objeto
		Vector3 targetPosition = startPosition + transform.forward * distance; // Posición objetivo

		float elapsedTime = 0f;
		float journeyLength = Vector3.Distance(startPosition, targetPosition);

		while (elapsedTime < journeyLength / speed)
		{
			if (IsColliderInFront())
			{
				break;
			}
			
			// Interpolar la posición
			transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime * speed) / journeyLength);
			elapsedTime += Time.deltaTime;
			yield return null; // Espera un frame
		}

		// Asegurarse de llegar a la posición final
		//transform.position = targetPosition;
		
	}
	
	
	// Distancia a la que se detectará el collider frente al jugador
	[SerializeField] private float detectionDistance = 1.0f;
	// Capa de los objetos que deben ser detectados
	[SerializeField] private LayerMask obstacleLayer;
	public bool IsColliderInFront()
	{
		// Lanzamos un Raycast en la dirección en la que el jugador está mirando
		RaycastHit hit;
		Vector3 forwardDirection = transform.TransformDirection(Vector3.forward);

		// Verifica si hay un objeto en frente del jugador (no se dibuja aquí)
		if (Physics.Raycast(transform.position+ new Vector3(0,_upChecker,0), forwardDirection, out hit, detectionDistance, obstacleLayer))
		{
			// Si el objeto tiene un Collider 3D que no es Trigger, no moveremos al jugador
			if (hit.collider != null && !hit.collider.isTrigger)
			{
				return true; // Hay un collider no trigger delante
			}
		}
		return false; // No hay ningún collider no trigger delante
	}	
	
	private void OnDrawGizmos()
	{
		// Asegúrate de que el objeto está seleccionado y activado
		if (enabled)
		{
			// Configuramos el color del Gizmo
			Gizmos.color = Color.blue;

			// Dirección en la que se lanza el raycast
			Vector3 forwardDirection = transform.TransformDirection(Vector3.forward);
			// Dibujamos el Gizmo en el editor (un rayo)
			Gizmos.DrawRay(transform.position+ new Vector3(0,_upChecker,0), forwardDirection * detectionDistance);
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
				rotationSpeedDelay * Time.deltaTime
			);

			// Espera al siguiente frame antes de continuar
			yield return null;
		}
	}
	
}
