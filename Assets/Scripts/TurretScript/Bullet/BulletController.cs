using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private int damage = 20;
    [SerializeField] private Rigidbody rb;

    private float _timer;

    private void OnEnable()
    {
        _timer = lifeTime;
    }

    public void Shoot(Vector3 dir, float speed)
    {
        transform.rotation = Quaternion.LookRotation(dir);
        rb.linearVelocity = dir.normalized * speed;
        _timer = lifeTime;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
            Deactivate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(damage);
            Deactivate();
        }
    }

    public void Deactivate()
    {
        rb.linearVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}