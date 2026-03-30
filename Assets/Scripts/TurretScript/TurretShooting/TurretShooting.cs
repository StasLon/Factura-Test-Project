using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class TurretShooting : MonoBehaviour
{
    [SerializeField] private BulletController prefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform container;

    [SerializeField] private int poolSize = 20;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private float bulletSpeed;

    private readonly List<BulletController> _pool = new();

    private bool _canShoot;
    private float _nextFireTime;

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var b = Instantiate(prefab, container);
            b.gameObject.SetActive(false);
            _pool.Add(b);
        }
    }

    private void Update()
    {
        if (!_canShoot) return;

        if (Input.GetMouseButton(0))
            TryShoot();
    }

    private void TryShoot()
    {
        if (Time.time < _nextFireTime) return;

        var bullet = Get();

        if (bullet == null) return;

        bullet.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
        bullet.gameObject.SetActive(true);
        bullet.Shoot(firePoint.forward, bulletSpeed);

        _nextFireTime = Time.time + fireRate;
    }

    private BulletController Get()
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            if (!_pool[i].gameObject.activeInHierarchy)
                return _pool[i];
        }

        return null;
    }

    public async UniTaskVoid EnableWithDelay(float delay = 0.3f)
    {
        await UniTask.Delay((int)(delay * 1000));
        _canShoot = true;
    }

    public void Disable() => _canShoot = false;
}