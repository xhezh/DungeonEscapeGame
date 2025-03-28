using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
    public GameObject[] enemyPrefabs;
    public float spawnInterval = 1f;
    public int initialEnemiesPerWave = 5;
    public int maxWaves = 10;

    private int currentWave = 0;
    private int enemiesRemaining = 0;
    private bool isSpawningWave = false;

    public GameUIManager uiManager;
    public Transform player;
    public float activationRadius = 10f;

    void Update() {
        if (player != null && Vector3.Distance(transform.position, player.position) <= activationRadius) {
            if (!isSpawningWave && currentWave < maxWaves) {
                StartCoroutine(SpawnWave());
            }
        }
    }

    IEnumerator SpawnWave() {
        isSpawningWave = true;
        currentWave++;
        int waveEnemies = initialEnemiesPerWave + (currentWave - 1) * 2;
        enemiesRemaining = waveEnemies;

        if (uiManager != null) {
            uiManager.UpdateWave(currentWave, maxWaves);
        }

        for (int i = 0; i < waveEnemies; i++) {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }

        while (enemiesRemaining > 0) {
            yield return null;
        }

        isSpawningWave = false;
    }

    void SpawnEnemy() {
        if (enemyPrefabs.Length == 0) return;
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemy = Instantiate(enemyPrefabs[randomIndex], transform.position, Quaternion.identity);

        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        if (enemyController != null) {
            enemyController.spawner = this;
        }
    }

    public void OnEnemyDestroyed() {
        enemiesRemaining--;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, activationRadius);
    }
}
