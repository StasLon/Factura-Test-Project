using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TurretShooting : MonoBehaviour
{
    [SerializeField] private BulletController bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform bulletBucket;
    [SerializeField] private int poolSize = 20;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private float bulletSpeed;
    private List<BulletController> bulletPool = new List<BulletController>();
    public bool canShoot;
    private float nextFireTime;

    private void Start()
    {
        CreateBulletPool();
    }

    private void CreateBulletPool()
    {
        bulletPool.Clear();

        for (int i = 0; i < poolSize; i++)
        {
            var bulletObj = Instantiate(bulletPrefab, bulletBucket);
            bulletObj.Deactivate();
            bulletPool.Add(bulletObj);
        }
    }

    private void Shoot()
    {
        if (Time.time < nextFireTime)
        {
            return;
        }

        var bullet = GetAvailableBullet();

        if (bullet == null)
        {
            return;
        }

        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        var shootDir = firePoint.forward;
        bullet.gameObject.SetActive(true);
        bullet.Shoot(shootDir,bulletSpeed);
        nextFireTime = Time.time * fireRate;
    }

    private BulletController GetAvailableBullet()
    {
        foreach (var bullet in bulletPool)
        {
            if (!bullet.gameObject.activeInHierarchy)
            {
                return bullet;
            }
        }
        return null;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && canShoot)
        {
            Shoot();
        }
    }

    public void EnableShooting()
    {
        StartCoroutine(EnableShootingCor());
    }

    private IEnumerator EnableShootingCor()
    {
        yield return new WaitForSeconds(0.3f);
        canShoot = true;
    }
}