using UnityEngine;
using Zenject;

public class GameStarter : IGameStarter
{
    // Зависимости
    private readonly IPlayerSpawner playerSpawner;
    private readonly ILevelInitializer levelInitializer;
    private readonly IResetOrchestrator resetOrchestrator;
    private readonly ILevelManager levelManager;

    // Конструктор с инъекцией
    [Inject]
    public GameStarter(
        IPlayerSpawner playerSpawner,
        ILevelInitializer levelInitializer,
        IResetOrchestrator resetOrchestrator,
        ILevelManager levelManager)
    {
        this.playerSpawner = playerSpawner;
        this.levelInitializer = levelInitializer;
        this.resetOrchestrator = resetOrchestrator;
        this.levelManager = levelManager;
    }

    // Запуск игры
    public void StartGame()
    {
        int level = levelManager.GetSelectedLevel();
        if (level == -1)
        {
            Debug.LogWarning("Уровень не выбран.");
            return;
        }

        // Сброс всего
        resetOrchestrator.PerformReset();

        // Инициализация уровня
        levelInitializer.InitializeLevel(level);

        // Спавн игрока
        playerSpawner.SpawnPlayer(level);
    }
}