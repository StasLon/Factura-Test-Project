using System.Collections.Generic;
using UnityEngine;

public class VFXPool : MonoBehaviour
{
    [SerializeField] private GameObject normalDeathPrefab;
    [SerializeField] private GameObject collisionDeathPrefab;
    [SerializeField] private int initialPoolSize = 20;
    [SerializeField] private int maxPoolSize = 60;

    private readonly Dictionary<VFXType, Queue<IVFX>> _pools = new();
    private readonly List<IVFX> _activeVFX = new();

    private void Awake()
    {
        InitializePool(VFXType.DeathNormal, normalDeathPrefab);
        InitializePool(VFXType.DeathCollision, collisionDeathPrefab);
    }

    private void InitializePool(VFXType type, GameObject prefab)
    {
        if (prefab == null)
        {
            return;
        }

        if (!prefab.TryGetComponent<IVFX>(out _))
        {
            return;
        }

        _pools[type] = new Queue<IVFX>();

        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewVFX(type, prefab);
        }
    }

    private IVFX CreateNewVFX(VFXType type, GameObject prefab)
    {
        var go = Instantiate(prefab, transform);
        go.SetActive(false);

        var vfx = go.GetComponent<IVFX>();

        if (go.TryGetComponent(out DeathVFX dvfx))
            dvfx.Construct(this);

        _pools[type].Enqueue(vfx);
        return vfx;
    }

    public void PlayVFX(VFXType type, Vector3 position, Quaternion rotation = default)
    {
        if (!_pools.TryGetValue(type, out var queue) || queue == null)
        {
            return;
        }

        IVFX vfx;

        if (queue.Count > 0)
        {
            vfx = queue.Dequeue();
        }
        else if (_activeVFX.Count < maxPoolSize)
        {
            GameObject prefab = type == VFXType.DeathNormal ? normalDeathPrefab : collisionDeathPrefab;
            vfx = CreateNewVFX(type, prefab);
        }
        else
        {
            vfx = _activeVFX[0];
            ReturnToPool(vfx);
        }

        var mono = vfx as MonoBehaviour;
        if (mono != null)
        {
            mono.transform.SetPositionAndRotation(position, rotation);
            mono.gameObject.SetActive(true);
        }

        vfx.PlayAt(position, rotation);
        _activeVFX.Add(vfx);
    }

    public void ReturnToPool(IVFX vfx)
    {
        if (vfx == null) return;

        vfx.OnVFXFinished();

        var mono = vfx as MonoBehaviour;
        if (mono != null)
        {
            mono.gameObject.SetActive(false);
            mono.transform.SetParent(transform);
        }

        VFXType type = vfx.GetVFXType();

        if (_pools.TryGetValue(type, out var queue))
        {
            queue.Enqueue(vfx);
        }

        _activeVFX.Remove(vfx);
    }

    public void ReturnAllToPool()
    {
        foreach (var vfx in _activeVFX)
        {
            ReturnToPool(vfx);
        }
    }
}