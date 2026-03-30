using UnityEngine;

public interface IEnemy
{
    void Initialize(Vector3 position, Quaternion rotation);
    void Activate();
    void Deactivate();
    void OnSpawned();
    void OnDespawned();
    Transform GetEnemyTransform();
}