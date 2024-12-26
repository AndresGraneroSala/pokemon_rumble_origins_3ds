using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform modelReference;

    public void SetBillboard(float rotationModel)
    {
        // Cambia la rotación del eje X a 50 grados, manteniendo los valores actuales de Y y Z
        Vector3 currentRotation = transform.rotation.eulerAngles;
        currentRotation.x = 45;
        currentRotation.z = rotationModel * -1;
        transform.rotation = Quaternion.Euler(currentRotation);
    }

    void Start()
    {
       
    }
}