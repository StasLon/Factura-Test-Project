using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private EnemyPool _enemyPool;
    private VFXPool _vfxPool;

    private readonly HashSet<IEnemy> _allSpawnedEnemies = new();

    public void Initialize(EnemyPool enemyPool, VFXPool vfxPool)
    {
        _enemyPool = enemyPool;
        _vfxPool = vfxPool;

        if (_enemyPool == null) Debug.LogError("EnemySpawner: EnemyPool is null!");
        if (_vfxPool == null) Debug.LogError("EnemySpawner: VFXPool is null!");

        Debug.Log("EnemySpawner: Initialized successfully with both pools.");
    }

    public void SpawnEnemiesAtPoints(List<Transform> spawnPoints)
    {
        if (_enemyPool == null) return;

        foreach (var point in spawnPoints)
        {
            if (point == null) continue;

            Vector3 position = point.position + GetRandomOffset();
            Quaternion rotation = point.rotation;

            IEnemy enemy = _enemyPool.GetEnemy(position, rotation);
            if (enemy != null)
                _allSpawnedEnemies.Add(enemy);
        }

        Debug.Log($"EnemySpawner: Spawned {spawnPoints.Count} enemies.");
    }

    private Vector3 GetRandomOffset()
    {
        Vector2 r = Random.insideUnitCircle * 1.8f;
        return new Vector3(r.x, 0f, r.y);
    }

    // √лавный метод Ч теперь принимает тип VFX
    public void ReturnEnemy(IEnemy enemy, VFXType vfxType = VFXType.DeathNormal)
    {
        if (enemy == null || _enemyPool == null) return;

        if (_allSpawnedEnemies.Remove(enemy))
        {
            _enemyPool.ReturnToPool(enemy);

            if (_vfxPool != null)
            {
                var enemyTransform = enemy.GetEnemyTransform();
                var pos = new Vector3(enemyTransform.position.x, enemyTransform.position.y + 1f, enemyTransform.position.z);
                _vfxPool.PlayVFX(vfxType, pos, Quaternion.identity);
            }
        }
    }

    public void DespawnAllEnemies()
    {
        if (_enemyPool == null) return;

        foreach (var enemy in _allSpawnedEnemies)
            _enemyPool.ReturnToPool(enemy);

        _allSpawnedEnemies.Clear();

        if (_vfxPool != null)
            _vfxPool.ReturnAllToPool();

        Debug.Log("EnemySpawner: All enemies and VFX returned.");
    }
}