using UnityEngine.Events;

public interface IGameStateManager
{
    bool IsGameOver { get; }
    void SetGameOver(bool value);
    void SetLevelCompleted();  // Новый метод для завершения уровня
    event UnityAction<GameEndType> OnGameEnd;  // Изменено: теперь с параметром GameEndType
}