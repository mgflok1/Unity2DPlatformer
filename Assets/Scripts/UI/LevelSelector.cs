using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelSelector : MonoBehaviour
{
    // Индекс уровня
    [SerializeField] private int levelIndex;
    // Цвет выбранного
    [SerializeField] private Color selectedColor = Color.green;
    // Цвет невыбранного
    [SerializeField] private Color unselectedColor = new Color(1f, 0.647f, 0f);

    private Outline outline;
    private Button button;

    // Инъекция levelManager
    [Inject] private ILevelManager levelManager;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        button = GetComponent<Button>();
    }

    private void Start()
    {
        SetOutlineColor(unselectedColor);
        button.onClick.AddListener(OnLevelClicked);
        levelManager.OnLevelSelected += OnLevelSelected;
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnLevelClicked);
        levelManager.OnLevelSelected -= OnLevelSelected;
    }

    // Обработчик клика по уровню
    private void OnLevelClicked()
    {
        levelManager.SelectLevel(levelIndex);
    }

    // Обработчик выбора уровня
    private void OnLevelSelected(int selectedLevelIndex)
    {
        SetOutlineColor(selectedLevelIndex == levelIndex ? selectedColor : unselectedColor);
    }

    // Установка цвета контура
    private void SetOutlineColor(Color color)
    {
        if (outline != null)
            outline.effectColor = color;
    }
}