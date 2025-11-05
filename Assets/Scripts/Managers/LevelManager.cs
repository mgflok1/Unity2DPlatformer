using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class LevelManager : MonoBehaviour, ILevelManager
{
    private int selectedLevel = -1;

    // Событие выбора уровня
    public event UnityAction<int> OnLevelSelected;

    // Инъекция boundariesProvider
    [Inject] private ICameraBoundariesProvider boundariesProvider;

    // Выбор уровня
    public void SelectLevel(int levelIndex)
    {
        if (selectedLevel == levelIndex || !IsValidLevel(levelIndex)) return;

        selectedLevel = levelIndex;
        OnLevelSelected?.Invoke(selectedLevel);
    }

    // Получение выбранного уровня
    public int GetSelectedLevel() => selectedLevel;

    // Проверка выбора уровня
    public bool IsLevelSelected() => selectedLevel != -1;

    // Проверка валидности уровня
    private bool IsValidLevel(int levelIndex) =>
        boundariesProvider.GetLeftBoundary(levelIndex) != null;
}