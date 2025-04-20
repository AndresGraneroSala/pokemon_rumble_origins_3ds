using UnityEngine;

public class DirectMovement : MonoBehaviour {
    private Transform target;
    [SerializeField] private float speed = 5f, rotationSpeed = 150f;
    [SerializeField] private float obstacleDetectionRange = 2f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private Transform modelRotation;
    private Vector3[] _avoidDirections;
    private bool _shouldUpdate;

    public Transform ModelRotation { get { return modelRotation; } }

    void Start() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        _avoidDirections = new Vector3[] {
            Quaternion.Euler(0, -100, 0) * Vector3.forward,
            Quaternion.Euler(0, 100, 0) * Vector3.forward
        };
    }

    void Update() {
        if (target == null) return;

        Vector3 direction = (new Vector3(target.position.x, transform.position.y, target.position.z) - modelRotation.position).normalized;
        direction.y = 0;

        if (!Physics.Raycast(transform.position, direction, obstacleDetectionRange, obstacleLayer)) {
            transform.position += direction * speed * Time.deltaTime;
        } else {
            foreach (Vector3 avoidDir in _avoidDirections) {
                if (!Physics.Raycast(transform.position, avoidDir, obstacleDetectionRange, obstacleLayer)) {
                    transform.position += avoidDir.normalized * speed * Time.deltaTime;
                    break;
                }
            }
        }
        RotateModelTowards(direction);
    }

    private void RotateModelTowards(Vector3 direction) {
        if (direction != Vector3.zero && modelRotation != null) {
            modelRotation.rotation = Quaternion.RotateTowards(modelRotation.rotation, 
                Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        }
    }

    void OnDrawGizmos() {
        if (target == null) return;
        
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, target.position);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * obstacleDetectionRange);
    }
}