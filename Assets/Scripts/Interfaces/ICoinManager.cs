using UnityEngine.Events;

public interface ICoinManager
{
    int CurrentCoins { get; }
    int LevelCoins { get; }  // Новое: Собранные монеты на уровне
    int TotalCoinsOnLevel { get; }  // Новое: Общее количество монет на уровне
    void AddCoins(int value);
    void ResetCoinsForLevel();
    void InitializeLevelCoins(int totalOnLevel);  // Новое: Инициализация общего количества на уровне
    event UnityAction<int> OnCoinsChanged;
    event UnityAction<int, int> OnLevelCoinsChanged;  // Новое: Событие для изменений монет на уровне
}