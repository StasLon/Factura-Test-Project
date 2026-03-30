using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    [SerializeField] private float maxHealth = 20f;
    private float _currentHealth;
    

    public void Initialize(Vector3 position, Quaternion rotation)
    {
        _currentHealth = maxHealth;
    }

    public void Activate()
    {
        Debug.Log($"Enemy: Activated at {transform.position}");
    }

    public void Deactivate()
    {
        Debug.Log("Enemy: Deactivated.");
    }

    public void OnSpawned()
    {
        Debug.Log("Enemy: OnSpawned called.");
    }

    public void OnDespawned()
    {
        Debug.Log("Enemy: OnDespawned called.");
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        FindAnyObjectByType<EnemySpawner>()?.ReturnEnemy(this); // временно, лучше через событие позже
        Debug.Log("Enemy: Died and returned to pool.");
    }

    public Transform GetEnemyTransform()
    {
        return transform;
    }
}