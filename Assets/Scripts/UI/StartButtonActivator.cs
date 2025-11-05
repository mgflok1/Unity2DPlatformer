using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StartButtonActivator : MonoBehaviour
{
    // Кнопка старта
    [SerializeField] private Button startButton;

    // Инъекции зависимостей
    [Inject] private ILevelManager levelManager;
    [Inject] private IGameStarter gameStarter;
    [Inject] private IUIManager uiManager;

    private void Start()
    {
        startButton.interactable = false;
        levelManager.OnLevelSelected += OnLevelSelected;
        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    private void OnDestroy()
    {
        levelManager.OnLevelSelected -= OnLevelSelected;
        startButton.onClick.RemoveListener(OnStartButtonClicked);
    }

    // Обработчик выбора уровня
    private void OnLevelSelected(int selectedLevel)
    {
        startButton.interactable = levelManager.IsLevelSelected();
    }

    // Обработчик клика по кнопке старта
    private void OnStartButtonClicked()
    {
        gameStarter.StartGame();
        uiManager.ShowLevelSelectionPanel(false);
        uiManager.ShowGamePanel(true);
    }
}