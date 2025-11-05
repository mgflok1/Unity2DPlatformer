public interface IGameManager
{
    bool IsGameOver { get; }
    void PlayerDied();
    void RestartGame();
    void ExitToMenu();
}