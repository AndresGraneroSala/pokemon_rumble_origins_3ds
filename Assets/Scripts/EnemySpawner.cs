using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject enemyPrefab;      // Prefab del enemigo
    public int enemiesToSpawn = 10;     // Cuántos enemigos spawnear
    public float spawnInterval = 2f;    // Intervalo entre spawns

    [Header("Spawn Area")]
    public Vector2 spawnAreaSize = new Vector2(10f, 10f); // Área de spawn


    void Start()
    {
        InvokeRepeating("CountSpawns", 0f, spawnInterval);
    }

    void CountSpawns()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        

        Vector3 randomPos = transform.position + new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            0,
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2)
        );

        Instantiate(enemyPrefab, randomPos, enemyPrefab.transform.rotation);
    }

    void OnDrawGizmosSelected()
    {
        // Dibuja el área de spawn en el editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnAreaSize.x, 0.1f, spawnAreaSize.y));
    }
}