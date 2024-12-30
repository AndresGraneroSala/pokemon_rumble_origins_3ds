using System;
using UnityEngine;

public class DirectMovement : MonoBehaviour
{
    private Transform target; // Objetivo al que el agente debe moverse
    [SerializeField] private float speed = 5f, rotationSpeed=150f; // Velocidad de movimiento
    [SerializeField] private float obstacleDetectionRange = 2f; // Distancia para detectar obstáculos
    [SerializeField] private LayerMask obstacleLayer; // Capa de los obstáculos

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (target == null)
            return;

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0; // Asegurar que no haya movimiento en el eje Y

        // Detectar obstáculos con un Raycast
        if (!Physics.Raycast(transform.position, direction, obstacleDetectionRange, obstacleLayer))
        {
            // Sin obstáculos, moverse hacia el objetivo
            transform.position += direction * speed * Time.deltaTime;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // Detectar un camino alternativo
            AvoidObstacle(direction);
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void AvoidObstacle(Vector3 direction)
    {
        // Intentar girar a la izquierda o a la derecha para evitar el obstáculo
        Vector3 left = Quaternion.Euler(0, -45, 0) * direction;
        Vector3 right = Quaternion.Euler(0, 45, 0) * direction;

        if (!Physics.Raycast(transform.position, left, obstacleDetectionRange, obstacleLayer))
        {
            transform.position += left.normalized * speed * Time.deltaTime;
        }
        else if (!Physics.Raycast(transform.position, right, obstacleDetectionRange, obstacleLayer))
        {
            transform.position += right.normalized * speed * Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        if (target != null)
        {
            // Dibujar una línea hacia el objetivo
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.position);
        }

        // Dibujar el rango de detección de obstáculos
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * obstacleDetectionRange);
    }
}
