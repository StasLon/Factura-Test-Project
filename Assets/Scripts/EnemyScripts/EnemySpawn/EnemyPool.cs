using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialSize = 20;
    [SerializeField] private int maxSize = 100;

    private readonly Queue<IEnemy> _pool = new();
    private readonly List<IEnemy> _active = new();

    private void Awake()
    {
        for (int i = 0; i < initialSize; i++)
            Create();
    }

    private IEnemy Create()
    {
        var go = Instantiate(prefab, transform);
        go.SetActive(false);

        var enemy = go.GetComponent<IEnemy>();
        _pool.Enqueue(enemy);
        return enemy;
    }

    public IEnemy Get(Vector3 pos, Quaternion rot)
    {
        IEnemy enemy = _pool.Count > 0
            ? _pool.Dequeue()
            : (_active.Count < maxSize ? Create() : _active[0]);

        var mono = (MonoBehaviour)enemy;
        mono.transform.SetPositionAndRotation(pos, rot);
        mono.gameObject.SetActive(true);

        enemy.Initialize(pos, rot);
        enemy.Activate();
        enemy.OnSpawned();

        _active.Add(enemy);
        return enemy;
    }

    public void Return(IEnemy enemy)
    {
        enemy.Deactivate();
        enemy.OnDespawned();

        var mono = (MonoBehaviour)enemy;
        mono.gameObject.SetActive(false);

        _active.Remove(enemy);
        _pool.Enqueue(enemy);
    }
}