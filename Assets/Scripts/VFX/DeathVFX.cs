using UnityEngine;

public class DeathVFX : MonoBehaviour, IVFX
{
    [SerializeField] private VFXType vfxType = VFXType.DeathNormal;

    private ParticleSystem _particleSystem;
    private float _duration;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        if (_particleSystem != null)
        {
            var main = _particleSystem.main;
            _duration = main.duration + main.startLifetime.constantMax + 0.5f;
        }
    }

    public void PlayAt(Vector3 position, Quaternion rotation)
    {
        transform.SetPositionAndRotation(position, rotation);
        if (_particleSystem != null)
            _particleSystem.Play();

        Invoke(nameof(AutoReturn), _duration);
    }

    public void OnVFXFinished()
    {
        if (_particleSystem != null)
            _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public VFXType GetVFXType() => vfxType;

    private void AutoReturn()
    {
        FindAnyObjectByType<VFXPool>()?.ReturnToPool(this);
    }
}