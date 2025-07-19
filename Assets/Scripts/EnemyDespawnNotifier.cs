using UnityEngine;

public class EnemyDespawnNotifier : MonoBehaviour
{
    [HideInInspector] public EnemySpawner spawner;

    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.NotifyEnemyDestroyed();
        }
    }
}
