using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;
using TMPro;

public class CoinsUI : MonoBehaviour
{
    // Текст для отображения монет
    [SerializeField] private TMP_Text coinsText;

    // Инъекция coinManager
    [Inject] private ICoinManager coinManager;

    private void Start()
    {
        if (coinsText == null)
        {
            Debug.LogError("Текст монет не назначен!");
            return;
        }

        if (coinManager == null)
        {
            Debug.LogError("ICoinManager не инъектирован!");
            return;
        }

        // Подписка на изменение монет
        coinManager.OnCoinsChanged += UpdateCoinsDisplay;
        UpdateCoinsDisplay(coinManager.CurrentCoins);
    }

    private void OnDestroy()
    {
        if (coinManager != null)
        {
            // Отписка от события
            coinManager.OnCoinsChanged -= UpdateCoinsDisplay;
        }
    }

    // Обновление отображения монет
    private void UpdateCoinsDisplay(int newCoins)
    {
        coinsText.text = newCoins.ToString();
    }
}