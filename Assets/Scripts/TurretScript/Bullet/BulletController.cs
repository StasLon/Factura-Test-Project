using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private int bulletDamage = 20;
    [SerializeField] private Rigidbody rb;
    private float timer;

    private void OnEnable()
    {
        timer = lifeTime;
    }
    public void Deactivate()
    {
        rb.linearVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }

    public void Shoot(Vector3 dir, float bulletSpeed)
    {
        transform.rotation = Quaternion.LookRotation(dir);
        rb.linearVelocity = dir.normalized * bulletSpeed;
        timer = lifeTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Deactivate(); 
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Deactivate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();

        if (enemy != null)
        {
            enemy.TakeDamage(bulletDamage);
            Deactivate();
        }
    }


}
