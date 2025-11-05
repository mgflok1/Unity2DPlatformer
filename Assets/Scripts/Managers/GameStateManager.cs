using UnityEngine;
using UnityEngine.Events;
using Zenject;

public enum GameEndType
{
    None,
    Lose,
    Win
}

public class GameStateManager : MonoBehaviour, IGameStateManager
{
    // Инъекция uiManager
    [Inject] private IUIManager uiManager;

    private bool gameOver = false;
    private GameEndType currentEndType = GameEndType.None;

    // Событие окончания игры
    public event UnityAction<GameEndType> OnGameEnd;

    // Свойство проверки окончания игры
    public bool IsGameOver => gameOver;

    // Установка окончания игры
    public void SetGameOver(bool value)
    {
        if (!value)
        {
            gameOver = false;
            currentEndType = GameEndType.None;
            return;
        }

        SetGameEnd(GameEndType.Lose);
    }

    // Установка завершения уровня
    public void SetLevelCompleted()
    {
        SetGameEnd(GameEndType.Win);
    }

    // Установка типа окончания игры
    private void SetGameEnd(GameEndType type)
    {
        if (gameOver || type == GameEndType.None) return;

        gameOver = true;
        currentEndType = type;

        uiManager.ShowGameOverPanel(true);
        OnGameEnd?.Invoke(type);
    }
}