using UnityEngine;
using Zenject;

public class UIManager : MonoBehaviour, IUIManager
{
    // Панель окончания игры
    [SerializeField] private GameObject gameOverPanel;
    // Панель выбора уровня
    [SerializeField] private GameObject levelSelectionPanel;
    // Панель игры
    [SerializeField] private GameObject gamePanel;

    // Инъекция gameStateManager
    [Inject] private IGameStateManager gameStateManager;

    private void Start()
    {
        ShowGameOverPanel(false);
        ShowGamePanel(false);
        ShowLevelSelectionPanel(true);
    }

    // Показать/скрыть панель окончания игры
    public void ShowGameOverPanel(bool show)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(show);
        }
    }

    // Показать/скрыть панель выбора уровня
    public void ShowLevelSelectionPanel(bool show)
    {
        if (levelSelectionPanel != null)
        {
            levelSelectionPanel.SetActive(show);
        }
    }

    // Показать/скрыть игровую панель
    public void ShowGamePanel(bool show)
    {
        if (gamePanel != null)
        {
            gamePanel.SetActive(show);
        }
    }
}