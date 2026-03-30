using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CarHealth : MonoBehaviour
{
    [SerializeField] GameUIManager gameUi;
    [SerializeField] private GameObject lowHealthVFX;
    [SerializeField] private int lowHealthThreshold = 40;
    [SerializeField] private Image healthBarIMG;
    [SerializeField] private float tweenDuration = 0.35f;
    [SerializeField] private Ease easeType = Ease.OutQuint;

    public int maxHealth = 100;
    private int currentHealth = 100;
    public Renderer carRenderer;

    private Color originalColor;
    private Tween healthTween;
    private bool healthBarActivated = false;
    private float flashDuration = 0.7f;

    public int CurrentHealth => currentHealth;

    void Start()
    {
        currentHealth = maxHealth;

        if (carRenderer != null)
            originalColor = carRenderer.material.color;

        if (lowHealthVFX != null)
            lowHealthVFX.SetActive(false);

        if (healthBarIMG != null)
            healthBarIMG.fillAmount = 1f;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("HP: " + currentHealth);

        UpdateHealthBar();

        if (carRenderer != null)
            FlashWhite();

        if (currentHealth <= 0)
            Die();

        if (lowHealthVFX != null && currentHealth <= lowHealthThreshold)
            lowHealthVFX.SetActive(true);
    }

    private void UpdateHealthBar()
    {
        float targetFill = (float)currentHealth / maxHealth;

        if (healthBarIMG != null)
        {
            healthTween?.Kill();
            healthTween = DOTween.To(() => healthBarIMG.fillAmount,
                                     x => healthBarIMG.fillAmount = x,
                                     targetFill, tweenDuration)
                                 .SetEase(easeType);
        }
    }

    private void FlashWhite()
    {
        carRenderer.material.color = Color.white;
        DOTween.To(() => carRenderer.material.color,
                   x => carRenderer.material.color = x,
                   originalColor, flashDuration)
               .SetEase(Ease.OutQuad);
    }

    void Die()
    {
        gameUi.ShowLosePanel();
        Debug.Log("Машина уничтожена");
    }

    private void OnDestroy()
    {
        healthTween?.Kill();
    }
}