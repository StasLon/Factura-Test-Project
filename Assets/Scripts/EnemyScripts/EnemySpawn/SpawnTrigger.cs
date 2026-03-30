using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpawnTrigger : MonoBehaviour
{
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    private bool _hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _hasTriggered = true;

        if (spawner == null)
        {
            Debug.LogError("SpawnTrigger: Spawner is not assigned on " + gameObject.name);
            return;
        }

        Debug.Log($"SpawnTrigger: Player entered trigger {gameObject.name}. Spawning enemies...");
        spawner.SpawnEnemiesAtPoints(spawnPoints);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.92f, 0.016f, 0.7f);
        foreach (var point in spawnPoints)
        {
            if (point != null)
                Gizmos.DrawSphere(point.position, 0.8f);
        }
    }
}