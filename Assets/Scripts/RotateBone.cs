using UnityEngine;

public class RotateBone : MonoBehaviour {
    [SerializeField] private Vector3 maxRotationAngles = new Vector3(30f, 30f, 30f);
    [SerializeField, Range(0.1f, 1000f)] private float rotationSpeed = 250f;
    [SerializeField] private bool isAttack = false;
    private int animationFPS = 32;

    private float speedState = 1f;
    private Vector3 rotationDirection;
    private Vector3 currentRotation;
    private bool canAttack = false;
    private bool isBack = false;

    private float frameTimer = 0f;
    private float frameInterval;

    public bool IsAttack { get { return isAttack; } }

    void Start() {
        rotationDirection.x = maxRotationAngles.x > 0f ? 1f : 0f;
        rotationDirection.y = maxRotationAngles.y > 0f ? 1f : 0f;
        rotationDirection.z = maxRotationAngles.z > 0f ? 1f : 0f;

        frameInterval = 1f / (float)animationFPS;
    }

    public void SetSpeedState(float speed) {
        speedState = speed;
    }

    public void PlayAttack() {
        canAttack = true;
    }

    void FixedUpdate() {
        frameTimer += Time.fixedDeltaTime;
        if (frameTimer < frameInterval) return;

        frameTimer = 0f;

        if ((isAttack && !canAttack) || speedState <= 0f) return;

        float delta = rotationSpeed * frameInterval * speedState;

        currentRotation.x += rotationDirection.x * delta;
        currentRotation.y += rotationDirection.y * delta;
        currentRotation.z += rotationDirection.z * delta;

        if (Mathf.Abs(currentRotation.x) >= Mathf.Abs(maxRotationAngles.x)) {
            rotationDirection.x *= -1f;
            currentRotation.x = Mathf.Clamp(currentRotation.x, -maxRotationAngles.x, maxRotationAngles.x);
        }

        if (Mathf.Abs(currentRotation.y) >= Mathf.Abs(maxRotationAngles.y)) {
            rotationDirection.y *= -1f;
            currentRotation.y = Mathf.Clamp(currentRotation.y, -maxRotationAngles.y, maxRotationAngles.y);
        }

        if (Mathf.Abs(currentRotation.z) >= Mathf.Abs(maxRotationAngles.z)) {
            rotationDirection.z *= -1f;
            currentRotation.z = Mathf.Clamp(currentRotation.z, -maxRotationAngles.z, maxRotationAngles.z);
        }

        transform.localRotation = Quaternion.Euler(currentRotation);

        if (isAttack) StopAttacking();
    }

    private void StopAttacking() {
        if (!isBack &&
            (rotationDirection.x == -1f || rotationDirection.y == -1f || rotationDirection.z == -1f)) {
            isBack = true;
        }

        Quaternion rot = transform.localRotation;

        if (isBack &&
            rot.x <= 0.01f &&
            rot.y <= 0.01f &&
            rot.z <= 0.01f) {

            canAttack = false;
            isBack = false;

            rotationDirection.x = maxRotationAngles.x > 0f ? 1f : 0f;
            rotationDirection.y = maxRotationAngles.y > 0f ? 1f : 0f;
            rotationDirection.z = maxRotationAngles.z > 0f ? 1f : 0f;

            currentRotation = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }

    public void Configure(Rotate rotate) {
        maxRotationAngles = rotate.MaxRotationAngles;
        rotationSpeed = rotate.RotationSpeed;
    }
}
