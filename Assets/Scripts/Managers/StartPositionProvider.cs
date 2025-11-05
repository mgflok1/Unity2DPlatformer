using UnityEngine;
using Zenject;

public class StartPositionProvider : MonoBehaviour, IStartPositionProvider
{
    // Массив стартовых позиций
    [SerializeField] private Transform[] startPositions;

    private void Awake()
    {
        ValidateArrays();
    }

    // Валидация массивов
    private void ValidateArrays()
    {
        if (startPositions.Length == 0) Debug.LogError("Стартовые позиции не назначены!");
    }

    // Получение стартовой позиции
    public Transform GetStartPosition(int levelIndex) =>
        IsValidLevel(levelIndex) ? startPositions[levelIndex - 1] : null;

    // Проверка валидности уровня
    private bool IsValidLevel(int levelIndex) =>
        levelIndex > 0 && levelIndex <= startPositions.Length;
}