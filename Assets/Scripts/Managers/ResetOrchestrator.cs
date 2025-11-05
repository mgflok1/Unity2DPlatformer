using System;
using UnityEngine;
using Zenject;

public class ResetOrchestrator : IResetOrchestrator
{
    // Инъекции зависимостей
    [Inject] private IResetManager resetManager;
    [Inject] private IPlayerHealth playerHealth;
    [Inject] private IGameStateManager gameStateManager;
    [Inject] private ICoinManager coinManager;

    // Выполнение сброса
    public void PerformReset()
    {
        // Сброс состояния игры
        gameStateManager.SetGameOver(false);

        // Сброс всех сбросаемых объектов
        try
        {
            foreach (var resettable in resetManager.GetResettables())
            {
                resettable?.Reset();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"ResetOrchestrator: Ошибка сброса объектов: {ex.Message}");
        }

        // Сброс здоровья игрока
        playerHealth.ResetHealth();

        // Сброс монет уровня
        coinManager.ResetCoinsForLevel();
    }
}