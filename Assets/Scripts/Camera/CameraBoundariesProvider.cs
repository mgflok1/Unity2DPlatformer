using UnityEngine;
using Zenject;

public class CameraBoundariesProvider : MonoBehaviour, ICameraBoundariesProvider
{
    // Массивы границ
    [SerializeField] private Transform[] leftBoundaries;
    [SerializeField] private Transform[] rightBoundaries;
    [SerializeField] private Transform[] bottomBoundaries;

    private void Awake()
    {
        ValidateArrays();
    }

    // Валидация массивов
    private void ValidateArrays()
    {
        if (leftBoundaries.Length == 0) Debug.LogError("Нет назначенных левых границ!");
        if (rightBoundaries.Length == 0) Debug.LogError("Нет назначенных правых границ!");
        if (bottomBoundaries.Length == 0) Debug.LogError("Нет назначенных нижних границ!");
    }

    // Получение левой границы
    public Transform GetLeftBoundary(int levelIndex) =>
        IsValidLevel(levelIndex) ? leftBoundaries[levelIndex - 1] : null;

    // Получение правой границы
    public Transform GetRightBoundary(int levelIndex) =>
        IsValidLevel(levelIndex) ? rightBoundaries[levelIndex - 1] : null;

    // Получение нижней границы
    public Transform GetBottomBoundary(int levelIndex) =>
        IsValidLevel(levelIndex) ? bottomBoundaries[levelIndex - 1] : null;

    // Проверка валидности уровня
    private bool IsValidLevel(int levelIndex) =>
        levelIndex > 0 && levelIndex <= leftBoundaries.Length;
}