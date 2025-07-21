using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject[] enemyPrefabs; // Cambiado a un array de prefabs
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int maxEnemies = 10;

    [Header("References")]
    [SerializeField] private Transform playerTarget; // Asignar el jugador en el inspector
    [SerializeField] private GameController gameController; // Asignar el GameController en el inspector
    [SerializeField] private FeedbackManager feedbackManager; // Asignar el FeedbackManager en el inspector
    [SerializeField] private FloatingFeedbackText floatingFeedback; // Asignar el prefab/script de feedback en el inspector

    private float timer = 0f;
    private int currentEnemies = 0;

    void Update()
    {
        if (enemyPrefabs.Length == 0 || spawnPoints.Length == 0) return;
        timer += Time.deltaTime;
        if (timer >= spawnInterval && currentEnemies < maxEnemies)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0) return; // Validar que haya prefabs disponibles
        int prefabIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject selectedPrefab = enemyPrefabs[prefabIndex];

        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];
        GameObject enemy = Instantiate(selectedPrefab, spawnPoint.position, spawnPoint.rotation);
        currentEnemies++;

        // Añadir componente auxiliar para notificar destrucción
        EnemyDespawnNotifier notifier = enemy.AddComponent<EnemyDespawnNotifier>();
        notifier.spawner = this;

        // Asignar referencias al EnemyAI usando Setup
        EnemyAI ai = enemy.GetComponent<EnemyAI>();
        if (ai != null)
        {
            ai.Setup(gameController, playerTarget, feedbackManager, floatingFeedback);
        }
    }

    public void NotifyEnemyDestroyed()
    {
        currentEnemies--;
        if (currentEnemies < 0) currentEnemies = 0;
    }
}
