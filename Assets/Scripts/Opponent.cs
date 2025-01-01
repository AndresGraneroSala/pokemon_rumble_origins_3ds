using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Opponent : MonoBehaviour {

    [SerializeField] private Transform target;
    
    [SerializeField] private float distanceToAttack=1;
    [SerializeField] private float distanceToMove=10;
    
    [SerializeField] private Attack attack;
    private RotateBone [] _bones;
    
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

    private DirectMovement _directMovement;
    
    public Transform Model
    {
	    get { return _directMovement.ModelRotation; }
    }

    // Use this for initialization
	void Start ()
	{

		_playAttack = GetComponent<PlayAttack>();
		
		target = GameObject.FindGameObjectWithTag("Player").transform;
		
		_bones = GetComponentsInChildren<RotateBone>()
			.Where(bone => !bone.IsAttack) // Filtra los que no son isAttack
			.ToArray();
		
		_playAttack.InitPool(attack);

		_directMovement = GetComponent<DirectMovement>();
		
	}
	
	// Update is called once per frame
	void Update()
	{

		if (_isAttacking)
		{
			return;
		}
			_distanceToPlayer = Vector3.Distance(transform.position, target.position);
			

		if (_distanceToPlayer <= distanceToMove) 
		{
			if (_distanceToPlayer <= distanceToAttack) 
			{
				// Detenemos al agente cuando está dentro del rango de ataque
				_directMovement.enabled = false;
				StartCoroutine(_playAttack.Play(attack,1));
			}
			else
			{
				// Si está dentro del rango de persecución pero no de ataque, el agente sigue persiguiendo
				_directMovement.enabled = true;

			}
		}
		else
		{
			// Si el jugador se aleja del rango de persecución, detenemos al agente
			_directMovement.enabled = false;
		}
	}

	public void ChangeSpeedBones(float speed)
	{
		foreach (var bone in _bones)
		{
			bone.SetSpeedState(speed);
		}
	}
	
	
	// Distancia a la que se detectará el collider frente al jugador
	[SerializeField] private float detectionDistance = 1.0f;
	// Capa de los objetos que deben ser detectados
	[SerializeField] private LayerMask obstacleLayer;
	public bool IsColliderInDirection(Vector3 direction)
	{
		// Lanzamos un Raycast en la dirección especificada
		RaycastHit hit;
		Vector3 worldDirection = transform.TransformDirection(direction);

		// Verifica si hay un objeto en la dirección indicada (no se dibuja aquí)
		if (Physics.Raycast(transform.position + new Vector3(0, _upChecker, 0), worldDirection, out hit, detectionDistance, obstacleLayer))
		{
			// Si el objeto tiene un Collider 3D que no es Trigger, no moveremos al jugador
			if (hit.collider != null && !hit.collider.isTrigger)
			{
				return true; // Hay un collider no trigger en la dirección
			}
		}
		return false; // No hay ningún collider no trigger en la dirección
	}

	private void OnDrawGizmos()
	{
		// Asegúrate de que el objeto está seleccionado y activado
		if (enabled)
		{
			// Configuramos el color del Gizmo
			Gizmos.color = Color.blue;

			// Direcciones en las que se lanzarán los rayos
			Vector3[] directions = new Vector3[] { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

			foreach (Vector3 direction in directions)
			{
				Vector3 worldDirection = transform.TransformDirection(direction);
				// Dibujamos el Gizmo en el editor (un rayo por cada dirección)
				Gizmos.DrawRay(transform.position + new Vector3(0, _upChecker, 0), worldDirection * detectionDistance);
			}
		}
	}

// Uso del método para comprobar todas las direcciones se usa en otro script
	public bool IsColliderInAnyDirection()
	{
		return IsColliderInDirection(Vector3.forward) ||
		       IsColliderInDirection(Vector3.back) ||
		       IsColliderInDirection(Vector3.left) ||
		       IsColliderInDirection(Vector3.right);
	}
	
}
