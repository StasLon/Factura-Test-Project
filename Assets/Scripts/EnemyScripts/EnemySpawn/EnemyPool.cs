using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int initialPoolSize = 20;
    [SerializeField] private int maxPoolSize = 100;

    private readonly Queue<IEnemy> _pool = new();
    private readonly List<IEnemy> _activeEnemies = new();

    private void Awake()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("EnemyPool: enemyPrefab is not assigned!");
            return;
        }
        if (!enemyPrefab.TryGetComponent<IEnemy>(out _))
        {
            Debug.LogError("EnemyPool: enemyPrefab does not have IEnemy component!");
            return;
        }

        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewEnemy();
        }

        Debug.Log($"EnemyPool: Created initial pool of {initialPoolSize} enemies.");
    }

    private IEnemy CreateNewEnemy()
    {
        GameObject instance = Instantiate(enemyPrefab, transform);
        
        if (instance != null)
        {
            instance.SetActive(false);
        }

        if (!instance.TryGetComponent(out IEnemy enemy))
        {
            Debug.LogError("EnemyPool: Failed to get IEnemy from instantiated object!");
            Destroy(instance);
            return null;
        }

        _pool.Enqueue(enemy);
        return enemy;
    }

    public IEnemy GetEnemy(Vector3 position, Quaternion rotation)
    {
        IEnemy enemy;

        if (_pool.Count > 0)
        {
            enemy = _pool.Dequeue();
            Debug.Log("EnemyPool: Reused enemy from pool.");
        }
        else if (_activeEnemies.Count < maxPoolSize)
        {
            enemy = CreateNewEnemy();
            Debug.Log("EnemyPool: Created new enemy (pool was empty).");
        }
        else
        {
            enemy = _activeEnemies[0];
            ReturnToPool(enemy);
            Debug.LogWarning("EnemyPool: Pool limit reached, reused oldest enemy.");
        }

        var mono = enemy as MonoBehaviour;
        if (mono != null)
        {
            mono.transform.SetPositionAndRotation(position, rotation);
            mono.gameObject.SetActive(true);
        }

        enemy.Initialize(position, rotation);
        enemy.Activate();
        enemy.OnSpawned();

        _activeEnemies.Add(enemy);
        Debug.Log($"EnemyPool: Enemy activated at position {position}");

        return enemy;
    }

    public void ReturnToPool(IEnemy enemy)
    {
        if (enemy == null) return;

        enemy.Deactivate();
        enemy.OnDespawned();

        var mono = enemy as MonoBehaviour;
        if (mono != null)
        {
            mono.gameObject.SetActive(false);
            mono.transform.SetParent(transform);
        }

        _activeEnemies.Remove(enemy);
        _pool.Enqueue(enemy);

        Debug.Log("EnemyPool: Enemy returned to pool.");
    }

    public void ReturnAllToPool()
    {
        foreach (var enemy in _activeEnemies)
        {
            ReturnToPool(enemy);
        }
        Debug.Log("EnemyPool: All enemies returned to pool.");
    }
}