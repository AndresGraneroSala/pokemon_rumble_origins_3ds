using System;
using System.Collections;
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

    [SerializeField] private bool isAttack=false;

    public bool IsAttack
    {
        get { return isAttack; }
    }

    private bool canAttack=false, isBack=false;
    
    private void Start()
    {
        rotationDirection = resetRotationDirection();
    }


    public void SetSpeedState(float speed)
    {
        speedState = speed;
    }

    [ContextMenu("Play")]
    public void PlayAttack()
    {
        canAttack=true;
    }
    
    private void FixedUpdate()
    {
        if (isAttack && !canAttack)
        {
            return;
        }
        
        
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
        
        if (isAttack)
        {
            StopAttacking();
        }
    }


    private const float Epsilon = 0.01f;
    private void StopAttacking()
    {
        if (rotationDirection.x == -1 || rotationDirection.y == -1 || rotationDirection.z == -1)
        {
            isBack = true;
        }

        if (isBack &&
            transform.localRotation.x <= Epsilon &&
            transform.localRotation.y <= Epsilon &&
            transform.localRotation.z <= Epsilon)
        {
            canAttack = false;
            isBack = false;

            rotationDirection = resetRotationDirection();
            transform.localRotation = Quaternion.identity;
        }

    }

    public void Configure(Rotate rotate)
    {
        maxRotationAngles = rotate.MaxRotationAngles;
        rotationSpeed = rotate.RotationSpeed;
    }
    

    Vector3 resetRotationDirection()
    {
        return  new Vector3(
            maxRotationAngles.x > 0 ? 1 : 0,
            maxRotationAngles.y > 0 ? 1 : 0,
            maxRotationAngles.z > 0 ? 1 : 0
        );
    }
    
}