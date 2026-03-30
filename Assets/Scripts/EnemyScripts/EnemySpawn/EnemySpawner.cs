using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyPool pool;
    [SerializeField] private VFXPool vfxPool;
    [SerializeField] private Money money;

    private readonly HashSet<Enemy> _alive = new();

    public void Spawn(List<Transform> points, Transform player, CarHealth health)
    {
        foreach (var p in points)
        {
            var enemy = pool.Get(p.position, p.rotation) as Enemy;

            if (enemy == null) continue;

            var controller = ((MonoBehaviour)enemy).GetComponent<EnemyController>();
            controller.Initialize(player, health);

            enemy.DeathCollision -= OnEnemyDied; 
            enemy.DeathCollision += OnEnemyDied;


            _alive.Add(enemy);
        }
    }

    private void OnEnemyDied(Enemy enemy, VFXType type)
    {
        enemy.DeathCollision -= OnEnemyDied;

        _alive.Remove(enemy);

        if (type == VFXType.DeathNormal)
            money?.Add(enemy.Reward);

        pool.Return(enemy);

        vfxPool?.PlayVFX(
            type,
            enemy.transform.position + Vector3.up,
            Quaternion.identity
        );
    }

    public void DespawnAll()
    {
        foreach (var e in _alive)
            pool.Return(e);

        _alive.Clear();
        vfxPool?.ReturnAllToPool();
    }

    public void StopAllEnemies()
    {
        foreach (var enemy in _alive)
        {
            var controller = ((MonoBehaviour)enemy).GetComponent<EnemyController>();
            if (controller != null)
                controller.enabled = false;

            if (((MonoBehaviour)enemy).TryGetComponent<Rigidbody>(out var rb))
                rb.linearVelocity = Vector3.zero;
        }
    }
}