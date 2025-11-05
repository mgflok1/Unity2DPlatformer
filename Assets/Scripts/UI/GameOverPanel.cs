using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;

public class GameOverPanel : MonoBehaviour
{
    // UI
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TMP_Text coinsCollectedText;
    [SerializeField] private TMP_Text reasonOfDeathText;

    // Инъекции зависимостей
    [Inject] private IGameStarter gameStarter;
    [Inject] private ILevelManager levelManager;
    [Inject] private IUIManager uiManager;
    [Inject] private IGameStateManager gameStateManager;
    [Inject] private ICoinManager coinManager;

    private void Awake()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnRestartClicked);
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitClicked);
        }

        // Подписка на события
        gameStateManager.OnGameEnd += HandleGameEnd;
        coinManager.OnLevelCoinsChanged += OnLevelCoinsUpdated;
    }

    private void OnDestroy()
    {
        if (restartButton != null)
        {
            restartButton.onClick.RemoveListener(OnRestartClicked);
        }

        if (exitButton != null)
        {
            exitButton.onClick.RemoveListener(OnExitClicked);
        }
        // Отписка от событий
        gameStateManager.OnGameEnd -= HandleGameEnd;
        coinManager.OnLevelCoinsChanged -= OnLevelCoinsUpdated;
    }

    // Обработчик рестарта
    private void OnRestartClicked()
    {
        uiManager.ShowGameOverPanel(false);
        gameStarter.StartGame();
        gameStateManager.SetGameOver(false);
    }

    // Обработчик выхода
    private void OnExitClicked()
    {
        uiManager.ShowGameOverPanel(false);
        uiManager.ShowGamePanel(false);
        gameStateManager.SetGameOver(false);
        uiManager.ShowLevelSelectionPanel(true);
    }

    // Обработка окончания игры
    private void HandleGameEnd(GameEndType type)
    {
        UpdateCoinsDisplay();

        if (reasonOfDeathText != null)
        {
            reasonOfDeathText.text = (type == GameEndType.Lose) ? "YOU LOSE" : "LEVEL PASSED";
        }
    }

    // Обновление отображения монет
    private void UpdateCoinsDisplay()
    {
        if (coinsCollectedText != null)
        {
            int collected = coinManager.LevelCoins;
            int total = coinManager.TotalCoinsOnLevel;
            coinsCollectedText.text = $"COINS COLLECTED: {collected}/{total}";
        }
    }

    // Обновление при изменении монет на уровне
    private void OnLevelCoinsUpdated(int collected, int total)
    {
        if (gameObject.activeSelf)
        {
            UpdateCoinsDisplay();
        }
    }
}