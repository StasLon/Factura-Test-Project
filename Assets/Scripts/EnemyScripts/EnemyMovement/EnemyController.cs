using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;        // Transform машины
    public CarHealth playerHealth;  // ссылка на скрипт CarHealth машины
    [SerializeField] private Animator animator;

    public float detectionDistance = 20f; // дистанция агрессии
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    public int damage = 20;
    private bool hasAttacked = false; // чтобы атаковать только один раз
    private bool isChasing = false;   // начал ли преследование

    private void Start()
    {
        playerHealth = FindAnyObjectByType<CarHealth>();
        player = playerHealth.gameObject.transform;
    }
    void Update()
    {
        if (player == null || playerHealth == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // начинаем преследовать только если игрок близко
        if (!isChasing && distance <= detectionDistance)
        {
            isChasing = true;

        }

        if (isChasing)
        {
            ChasePlayer();

            if (animator != null)
                animator.SetBool("Attacking", true);
            Debug.Log($"{animator.GetBool("Attacking")}");
        }
        else
        {
            if (animator != null)
                animator.SetBool("Attacking", false);
            Debug.Log($"{animator.GetBool("Attacking")}");
        }

    }

    void ChasePlayer()
    {
        // направление на игрока
        Vector3 direction = (player.position - transform.position).normalized;

        // плавный поворот
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // движение вперёд
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        
    }

    private void OnDisable()
    {
        isChasing = false;
        animator.SetBool("Attacking", false);
    }

      
    void OnCollisionEnter(Collision collision)
    {
        if (hasAttacked) return; // уже атаковал

        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            hasAttacked = true;

            var enemy = GetComponent<Enemy>();
            FindAnyObjectByType<EnemySpawner>()?.ReturnEnemy(enemy, VFXType.DeathCollision);
            gameObject.SetActive(false);

        }
    }
}