using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {
    public static EnemyManager Instance;

    [SerializeField] private LayerMask obstacleLayer;
    private List<Transform> obstacles = new List<Transform>();
    private List<DirectMovement> enemies = new List<DirectMovement>();
    private Vector3[] obstaclePositions;

    void Awake() {
        Instance = this;
        CacheStaticObstacles();
    }

    public void ChangeTarget(Transform target)
    {
        foreach (var directMovement in enemies)
        {
            directMovement.ChangeTarget(target);
        }
    }

    private void CacheStaticObstacles() {
        GameObject[] obstacleObjects = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obj in obstacleObjects) {
            if (((1 << obj.layer) & obstacleLayer) != 0) {
                obstacles.Add(obj.transform);
            }
        }
        obstaclePositions = new Vector3[obstacles.Count];
        for (int i = 0; i < obstacles.Count; i++) {
            obstaclePositions[i] = obstacles[i].position;
        }
    }

    // Modificar el método CheckPath
    public bool CheckPath(Vector3 enemyPos, Vector3 direction, float detectionRange) {
        // Usar Physics.Raycast con la dirección rotada desde modelRotation
        return Physics.Raycast(enemyPos, direction, detectionRange, obstacleLayer);
    }

    public Vector3 GetSeparationForce(DirectMovement requester, float radius) {
        Vector3 force = Vector3.zero;
        foreach (DirectMovement enemy in enemies) {
            if (enemy == requester) continue;
            if (Vector3.Distance(requester.transform.position, enemy.transform.position) < radius) {
                force += (requester.transform.position - enemy.transform.position).normalized;
            }
        }
        return force.normalized;
    }

    public void RegisterEnemy(DirectMovement enemy)
    {
        enemies.Add(enemy);
    }

    public void UnregisterEnemy(DirectMovement enemy)
    {
        enemies.Remove(enemy);
    }
}