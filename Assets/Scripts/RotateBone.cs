using System;
using UnityEngine;

public class RotateBone : MonoBehaviour
{
    // Vector3 para definir la rotación máxima deseada
    [SerializeField]
    private Vector3 maxRotationAngles = new Vector3(30f, 30f, 30f);

    // Velocidad de rotación
    [SerializeField, Range(0.1f, 1000f)]
    private float rotationSpeed = 250f;
    private float speedState = 1;

    // Dirección de rotación (1 para adelante, -1 para atrás)
    private Vector3 rotationDirection = Vector3.one;

    // Rotación acumulativa actual
    private Vector3 currentRotation = Vector3.zero;

    private void Start()
    {
        rotationDirection = new Vector3(
            maxRotationAngles.x > 0 ? 1 : 0,
            maxRotationAngles.y > 0 ? 1 : 0,
            maxRotationAngles.z > 0 ? 1 : 0
        );
    }


    public void SetSpeedState(float speed)
    {
        speedState = speed;
    }
    
    private void FixedUpdate()
    {
        // Calcula el cambio de rotación para este frame
        Vector3 deltaRotation = rotationDirection * (rotationSpeed * Time.deltaTime * speedState);
        currentRotation += deltaRotation;

        // Verifica si se ha alcanzado el límite máximo en cualquier eje y cambia la dirección
        if (Mathf.Abs(currentRotation.x) >= Mathf.Abs(maxRotationAngles.x))
        {
            rotationDirection.x *= -1;
            currentRotation.x = Mathf.Clamp(currentRotation.x, -maxRotationAngles.x, maxRotationAngles.x);
        }
        if (Mathf.Abs(currentRotation.y) >= Mathf.Abs(maxRotationAngles.y))
        {
            rotationDirection.y *= -1;
            currentRotation.y = Mathf.Clamp(currentRotation.y, -maxRotationAngles.y, maxRotationAngles.y);
        }
        if (Mathf.Abs(currentRotation.z) >= Mathf.Abs(maxRotationAngles.z))
        {
            rotationDirection.z *= -1;
            currentRotation.z = Mathf.Clamp(currentRotation.z, -maxRotationAngles.z, maxRotationAngles.z);
        }

        // Aplica la rotación al transform
        transform.localRotation = Quaternion.Euler(currentRotation);
    }
    
}