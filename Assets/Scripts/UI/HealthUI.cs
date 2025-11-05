using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;

public class HealthUI : MonoBehaviour
{
    // Изображение полоски здоровья
    [SerializeField] private Image healthBarImage;
    // Длительность анимации
    [SerializeField] private float tweenDuration = 0.5f;

    // Инъекции зависимостей
    [Inject] private IPlayerHealth playerHealth;
    [Inject] private IGameStateManager gameStateManager;

    private void Start()
    {
        if (healthBarImage == null)
        {
            Debug.LogError("Изображение полоски здоровья не назначено!");
            return;
        }

        // Подписка на изменение здоровья
        playerHealth.OnHealthChanged += UpdateHealthBar;
        healthBarImage.color = Color.green;
        UpdateHealthBar(1f);
    }

    private void OnDestroy()
    {
        // Отписка от события
        playerHealth.OnHealthChanged -= UpdateHealthBar;
    }

    // Обновление полоски здоровья
    private void UpdateHealthBar(float normalizedValue)
    {
        if (gameStateManager.IsGameOver) return;

        // Целевой цвет: от зелёного к красному
        Color targetColor = Color.Lerp(Color.green, Color.red, 1 - normalizedValue);

        // Анимация заполнения и цвета
        healthBarImage.DOFillAmount(normalizedValue, tweenDuration).SetEase(Ease.OutQuad);
        healthBarImage.DOColor(targetColor, tweenDuration).SetEase(Ease.OutQuad);
    }
}