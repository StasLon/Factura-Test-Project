using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class CarHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int lowHealthThreshold = 40;

    [Header("View")]
    [SerializeField] private Image healthBar;
    [SerializeField] private Renderer carRenderer;
    [SerializeField] private GameObject lowHealthVFX;

    [Header("Tween")]
    [SerializeField] private float tweenDuration = 0.35f;
    [SerializeField] private Ease ease = Ease.OutQuint;
    [SerializeField] private float flashDuration = 0.7f;

    private int _currentHealth;
    private Color _originalColor;
    private Tween _healthTween;

    public int CurrentHealth => _currentHealth;

    public event Action Died;
    public event Action<int, int> HealthChanged;

    private void Awake()
    {
        _currentHealth = maxHealth;

        if (carRenderer != null)
            _originalColor = carRenderer.material.color;

        if (lowHealthVFX != null)
            lowHealthVFX.SetActive(false);

        if (healthBar != null)
            healthBar.fillAmount = 1f;
    }

    public void TakeDamage(int damage)
    {
        if (_currentHealth <= 0) return;

        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, maxHealth);

        HealthChanged?.Invoke(_currentHealth, maxHealth);

        UpdateHealthBar();
        Flash();

        if (_currentHealth <= lowHealthThreshold)
            lowHealthVFX?.SetActive(true);

        if (_currentHealth == 0)
            Died?.Invoke();
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null) return;

        float target = (float)_currentHealth / maxHealth;

        _healthTween?.Kill();
        _healthTween = DOTween.To(
            () => healthBar.fillAmount,
            x => healthBar.fillAmount = x,
            target,
            tweenDuration
        ).SetEase(ease);
    }

    private void Flash()
    {
        if (carRenderer == null) return;

        carRenderer.material.color = Color.white;

        DOTween.To(
            () => carRenderer.material.color,
            x => carRenderer.material.color = x,
            _originalColor,
            flashDuration
        );
    }

    private void OnDestroy()
    {
        _healthTween?.Kill();
    }
}