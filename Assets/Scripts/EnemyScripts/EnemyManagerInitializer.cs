using UnityEngine;

public class EnemyManagerInitializer : MonoBehaviour
{
    private void Awake()
    {
        EnemyPool enemyPool = GetComponent<EnemyPool>();
        VFXPool vfxPool = GetComponent<VFXPool>();
        EnemySpawner spawner = GetComponent<EnemySpawner>();

        if (enemyPool == null || vfxPool == null || spawner == null)
        {
            Debug.LogError("EnemyManagerInitializer: Missing required components on Enemy Manager!");
            return;
        }

        spawner.Initialize(enemyPool, vfxPool);
        Debug.Log("EnemyManagerInitializer: All pools and spawner initialized successfully.");
    }
}