using UnityEngine;
using System;

public class Enemy : MonoBehaviour, IEnemy
{
    [SerializeField] private float maxHealth = 20f;
    [SerializeField] private int reward = 10;

    private float _currentHealth;
    private bool _isDead = false; 

    public event Action<Enemy> Died;                 
    public Action<Enemy, VFXType> DeathCollision; 
    public int Reward => reward;

    public void Initialize(Vector3 position, Quaternion rotation)
    {
        _currentHealth = maxHealth;
        _isDead = false;

        Died = null;
        DeathCollision = null;
    }

    public void Activate() { }
    public void Deactivate() { }
    public void OnSpawned() { }
    public void OnDespawned() { }

    public void TakeDamage(float damage)
    {
        if (_isDead) return;

        _currentHealth -= damage;

        if (_currentHealth <= 0f)
            Die();
    }

    public void ForceKill(VFXType type)
    {
        if (_isDead) return;
        _isDead = true;

        DeathCollision?.Invoke(this, type);
        DeathCollision?.Invoke(this, VFXType.DeathCollision);
    }

    private void Die()
    {
        if (_isDead) return;
        _isDead = true;

        Died?.Invoke(this);
        DeathCollision?.Invoke(this, VFXType.DeathNormal);
    } 

    public Transform GetEnemyTransform() => transform;
}