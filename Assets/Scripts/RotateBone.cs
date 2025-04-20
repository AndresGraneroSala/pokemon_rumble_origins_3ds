using UnityEngine;

public class RotateBone : MonoBehaviour {
    [SerializeField] private Vector3 maxRotationAngles = new Vector3(30f, 30f, 30f);
    [SerializeField, Range(0.1f, 1000f)] private float rotationSpeed = 250f;
    [SerializeField] private bool isAttack = false;
    private float speedState = 1;
    private Vector3 rotationDirection = Vector3.one;
    private Vector3 currentRotation = Vector3.zero;
    private bool canAttack = false, isBack = false;

    public bool IsAttack { get { return isAttack; } }

    void Start() {
        rotationDirection = new Vector3(
            maxRotationAngles.x > 0 ? 1 : 0,
            maxRotationAngles.y > 0 ? 1 : 0,
            maxRotationAngles.z > 0 ? 1 : 0
        );
    }

    public void SetSpeedState(float speed) {
        speedState = speed;
    }

    public void PlayAttack() {
        canAttack = true;
    }

    void FixedUpdate() {
        if ((isAttack && !canAttack) || speedState <= 0) return;

        Vector3 deltaRotation = rotationDirection * (rotationSpeed * Time.fixedDeltaTime * speedState);
        currentRotation += deltaRotation;

        for (int i = 0; i < 3; i++) {
            if (Mathf.Abs(currentRotation[i]) >= Mathf.Abs(maxRotationAngles[i])) {
                rotationDirection[i] *= -1;
                currentRotation[i] = Mathf.Clamp(currentRotation[i], -maxRotationAngles[i], maxRotationAngles[i]);
            }
        }

        transform.localRotation = Quaternion.Euler(currentRotation);
        
        if (isAttack) StopAttacking();
    }

    private void StopAttacking() {
        if (rotationDirection.x == -1 || rotationDirection.y == -1 || rotationDirection.z == -1) {
            isBack = true;
        }

        if (isBack && transform.localRotation.x <= 0.01f && 
            transform.localRotation.y <= 0.01f && 
            transform.localRotation.z <= 0.01f) {
            canAttack = false;
            isBack = false;
            rotationDirection = new Vector3(
                maxRotationAngles.x > 0 ? 1 : 0,
                maxRotationAngles.y > 0 ? 1 : 0,
                maxRotationAngles.z > 0 ? 1 : 0
            );
            transform.localRotation = Quaternion.identity;
        }
    }

    public void Configure(Rotate rotate) {
        maxRotationAngles = rotate.MaxRotationAngles;
        rotationSpeed = rotate.RotationSpeed;
    }
}