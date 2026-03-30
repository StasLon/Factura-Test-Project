using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float detectionDistance = 20f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private int damage = 20;
    [SerializeField] private Animator animator;

    private Transform _target;
    private CarHealth _targetHealth;

    private bool _isChasing;
    private bool _hasAttacked;

    public void Initialize(Transform target, CarHealth health)
    {
        _target = target;
        _targetHealth = health;
    }

    private void Update()
    {
        if (_target == null || _targetHealth == null) return;

        float distance = Vector3.Distance(transform.position, _target.position);

        if (!_isChasing && distance <= detectionDistance)
            _isChasing = true;

        if (_isChasing)
        {
            Chase();
            animator?.SetBool("Attacking", true);
        }
        else
        {
            animator?.SetBool("Attacking", false);
        }
    }

    private void Chase()
    {
        Vector3 dir = (_target.position - transform.position).normalized;

        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);

        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_hasAttacked) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            _targetHealth?.TakeDamage(damage);
            _hasAttacked = true;

            var enemy = GetComponent<Enemy>();
            enemy?.ForceKill(VFXType.DeathCollision);
        }
    }

    public void OnDisable()
    {
        _isChasing = false;
        _hasAttacked = false;
        animator?.SetBool("Attacking", false);
    }

  
}