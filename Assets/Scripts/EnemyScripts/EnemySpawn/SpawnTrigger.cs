using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpawnTrigger : MonoBehaviour
{
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private Transform player;
    [SerializeField] private CarHealth playerHealth;

    private bool _triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered) return;
        if (!other.CompareTag("Player")) return;

        _triggered = true;

        spawner.Spawn(spawnPoints, player, playerHealth);
    }
}