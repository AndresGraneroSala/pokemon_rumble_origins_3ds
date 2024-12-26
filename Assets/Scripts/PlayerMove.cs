using System;
using System.Linq;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Transform model;
    
    // Velocidad de movimiento del jugador
    [SerializeField] private float speed = 5.0f;
    private bool _isMoving = false;

    private RotateBone[] _bones;

    // Distancia a la que se detectará el collider frente al jugador
    [SerializeField] private float detectionDistance = 1.0f;
    // Capa de los objetos que deben ser detectados
    [SerializeField] private LayerMask obstacleLayer;
    private float _upChecker = 0.1f;
    public bool block = false;
    
    private void Start()
    {
        _bones = GetComponentsInChildren<RotateBone>()
            .Where(bone => !bone.IsAttack) // Filtra los que no son isAttack
            .ToArray();
        
        SetSpeed(0.25f);
    }

    void Update()
    {
        if (block)
        {
            return;
        }
        
        // Obtener entrada del teclado (WASD o flechas del teclado)
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        if (UnityEngine.N3DS.GamePad.CirclePad != Vector2.zero)
        {
            moveHorizontal = UnityEngine.N3DS.GamePad.CirclePad.x;
            moveVertical = UnityEngine.N3DS.GamePad.CirclePad.y;
        }

        // Crear un vector de movimiento
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Normalizar el vector de movimiento para mantener la velocidad constante en todas las direcciones
        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }

        // Verificar si hay un collider frente al jugador
        if (!IsColliderInFront())
        {
            // Si no hay un collider, mover al jugador
            if (movement != Vector3.zero && !_isMoving)
            {
                _isMoving = true;
                SetSpeed(1);
            }
            else if (movement == Vector3.zero && _isMoving)
            {
                _isMoving = false;
                SetSpeed(0.25f);
            }

            // Aplicar el movimiento al objeto del jugador
            transform.Translate(movement * (speed * Time.deltaTime), Space.World);
        }

        // Hacer que el jugador gire hacia donde se está moviendo instantáneamente
            if (movement != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
                model.rotation = toRotation;
            
        }
    }

    public bool IsColliderInFront()
    {
        // Lanzamos un Raycast en la dirección en la que el jugador está mirando
        RaycastHit hit;
        Vector3 forwardDirection = model.TransformDirection(Vector3.forward);

        // Verifica si hay un objeto en frente del jugador (no se dibuja aquí)
        if (Physics.Raycast(model.position+new Vector3(0,_upChecker,0), forwardDirection, out hit, detectionDistance, obstacleLayer))
        {
            // Si el objeto tiene un Collider 3D que no es Trigger, no moveremos al jugador
            if (hit.collider != null && !hit.collider.isTrigger)
            {
                return true; // Hay un collider no trigger delante
            }
        }
        return false; // No hay ningún collider no trigger delante
    }

    private void SetSpeed(float speed)
    {
        foreach (var bone in _bones)
        {
            bone.SetSpeedState(speed);
        }
    }

    // Este método se llama en el editor para dibujar los Gizmos en la vista de la escena
    private void OnDrawGizmos()
    {
        // Asegúrate de que el objeto está seleccionado y activado
        if (enabled)
        {
            // Configuramos el color del Gizmo
            Gizmos.color = Color.red;

            // Dirección en la que se lanza el raycast
            Vector3 forwardDirection = model.TransformDirection(Vector3.forward);

            // Dibujamos el Gizmo en el editor (un rayo)
            Gizmos.DrawRay(model.position+ new Vector3(0,_upChecker,0), forwardDirection * detectionDistance);
        }
    }
}
