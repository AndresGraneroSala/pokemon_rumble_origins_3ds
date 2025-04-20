using UnityEngine;

public class PlayerMove : MonoBehaviour {
    [SerializeField] private Transform model;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float detectionDistance = 1.0f;
    [SerializeField] private LayerMask obstacleLayer;
    private float _upChecker = 0.1f;
    private RotateBone[] _bones;
    private bool _isMoving = false;
    public bool block = false;

    void Start() {
        RotateBone[] allBones = GetComponentsInChildren<RotateBone>();
        _bones = new RotateBone[allBones.Length];
        int count = 0;
        foreach (RotateBone bone in allBones) {
            if (!bone.IsAttack) {
                _bones[count++] = bone;
            }
        }
        System.Array.Resize(ref _bones, count);
        SetSpeed(0.25f);
    }

    void Update() {
        if (block) return;

        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        if (UnityEngine.N3DS.GamePad.CirclePad != Vector2.zero) {
            moveHorizontal = UnityEngine.N3DS.GamePad.CirclePad.x;
            moveVertical = UnityEngine.N3DS.GamePad.CirclePad.y;
        }

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        if (movement.magnitude > 1) movement.Normalize();

        bool tryingToMove = movement != Vector3.zero;

        // Solo chequea colisión si intenta moverse
        if (tryingToMove) {
            Vector3 forwardDirection = movement.normalized;
            RaycastHit hit;
            bool blocked = Physics.Raycast(model.position + Vector3.up * _upChecker, forwardDirection, out hit, detectionDistance, obstacleLayer)
                           && hit.collider != null && !hit.collider.isTrigger;

            if (!blocked) {
                bool wasMoving = _isMoving;
                _isMoving = true;

                if (_isMoving != wasMoving) {
                    SetSpeed(1);
                }

                transform.Translate(movement * (speed * Time.deltaTime) * GameManager.instance.PlayerSpeed, Space.World);

                if (movement != Vector3.zero) {
                    model.rotation = Quaternion.LookRotation(movement, Vector3.up);
                }
            } else {
                if (_isMoving) {
                    _isMoving = false;
                    SetSpeed(0.25f);
                }
            }
        } else {
            if (_isMoving) {
                _isMoving = false;
                SetSpeed(0.25f);
            }
        }
    }

    public bool IsColliderInFront() {
        Vector3 forwardDirection = model.TransformDirection(Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(model.position + Vector3.up * _upChecker, forwardDirection, out hit, detectionDistance, obstacleLayer)) {
            return hit.collider != null && !hit.collider.isTrigger;
        }
        return false;
    }

    private void SetSpeed(float speed) {
        foreach (RotateBone bone in _bones) {
            bone.SetSpeedState(speed);
        }
    }

    void OnDrawGizmos() {
        if (!enabled) return;
        
        Gizmos.color = Color.red;
        Vector3 forwardDirection = model.TransformDirection(Vector3.forward);
        Gizmos.DrawRay(model.position + Vector3.up * _upChecker, forwardDirection * detectionDistance);
    }
}