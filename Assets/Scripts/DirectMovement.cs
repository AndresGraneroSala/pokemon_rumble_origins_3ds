using UnityEngine;

public class DirectMovement : MonoBehaviour {
    private Transform target;
    [SerializeField] private float speed = 5f, rotationSpeed = 150f;
    [SerializeField] private float obstacleDetectionRange = 2f;
    [SerializeField] private Transform modelRotation;
    [SerializeField] private float separationRadius = 1.5f;
    private Vector3[] _avoidDirections;
    
    void Start() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        EnemyManager.Instance.RegisterEnemy(this);
        
        _avoidDirections = new Vector3[] {
            Quaternion.Euler(0, -100, 0) * Vector3.forward,
            Quaternion.Euler(0, 100, 0) * Vector3.forward
        };
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
        GetComponent<Opponent>().ChangeTarget(newTarget);
    }

    void OnDestroy() {
        EnemyManager.Instance.UnregisterEnemy(this);
    }

    void Update() {
        if (target == null) return;

        Vector3 targetPosFlat = new Vector3(target.position.x, transform.position.y, target.position.z);
        Vector3 direction = (targetPosFlat - modelRotation.position).normalized; // Usar posición del modelo
        direction += EnemyManager.Instance.GetSeparationForce(this, separationRadius) * 0.3f;
        direction.y = 0;

        // Usar modelRotation.forward para el rayo y direcciones de evasión
        if (!EnemyManager.Instance.CheckPath(modelRotation.position, modelRotation.forward, obstacleDetectionRange)) {
            transform.position += direction.normalized * speed * Time.deltaTime;
        } else {
            foreach (Vector3 avoidDir in _avoidDirections) {
                Vector3 rotatedAvoidDir = modelRotation.TransformDirection(avoidDir); // Aplicar rotación del modelo
                if (!EnemyManager.Instance.CheckPath(modelRotation.position, rotatedAvoidDir, obstacleDetectionRange)) {
                    transform.position += rotatedAvoidDir.normalized * speed * Time.deltaTime;
                    direction = rotatedAvoidDir;
                    break;
                }
            }
        }
        RotateModelTowards(direction);
    }

    private void RotateModelTowards(Vector3 direction) {
        if (direction != Vector3.zero && modelRotation != null) {
            modelRotation.rotation = Quaternion.RotateTowards(
                modelRotation.rotation, 
                Quaternion.LookRotation(direction), 
                rotationSpeed * Time.deltaTime
            );
        }
    }
}