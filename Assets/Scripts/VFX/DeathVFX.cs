using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class DeathVFX : MonoBehaviour, IVFX
{
    [SerializeField] private VFXType type;

    private ParticleSystem _ps;
    private float _duration;
    private VFXPool _pool;

    private CancellationTokenSource _cts;

    public void Construct(VFXPool pool)
    {
        _pool = pool;
    }

    private void Awake()
    {
        _ps = GetComponent<ParticleSystem>();

        if (_ps != null)
        {
            var main = _ps.main;
            _duration = main.duration + main.startLifetime.constantMax;
        }
    }

    public void PlayAt(Vector3 position, Quaternion rotation)
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        transform.SetPositionAndRotation(position, rotation);
        _ps?.Play();

        ReturnAsync(_cts.Token).Forget();
    }

    private async UniTaskVoid ReturnAsync(CancellationToken token)
    {
        try
        {
            await UniTask.Delay((int)(_duration * 1000), cancellationToken: token);

            if (this == null || !gameObject.activeInHierarchy) return;

            _pool.ReturnToPool(this);
        }
        catch
        {
        }
    }

    public void OnVFXFinished()
    {
        _cts?.Cancel();
        _ps?.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public VFXType GetVFXType() => type;

    private void OnDisable()
    {
        _cts?.Cancel();
    }
}