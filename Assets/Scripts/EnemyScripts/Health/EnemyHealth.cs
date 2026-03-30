using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] private int currentHealth = 100;
    [SerializeField] private Money moneyScript;

   
    void Start()
    {
        moneyScript = FindAnyObjectByType<Money>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Enemy HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        moneyScript.AddGold(10);
        var enemy = GetComponent<Enemy>();
        FindAnyObjectByType<EnemySpawner>()?.ReturnEnemy(enemy);

        gameObject.SetActive(false);
    }
}