using UnityEngine;
using Zenject;

public class LevelInitializer : ILevelInitializer
{
    // Инъекции зависимостей
    [Inject] private ICoinRegistry coinRegistry;
    [Inject] private ICoinManager coinManager;

    // Инициализация уровня
    public void InitializeLevel(int levelIndex)
    {
        int totalCoinsOnLevel = coinRegistry.GetTotalCoinsForLevel(levelIndex);

        if (totalCoinsOnLevel == 0)
        {
            Debug.LogWarning($"Монеты не найдены для уровня {levelIndex} после расчёта");
        }

        coinManager.InitializeLevelCoins(totalCoinsOnLevel);
    }
}