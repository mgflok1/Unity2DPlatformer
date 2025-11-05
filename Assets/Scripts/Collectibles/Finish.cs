using UnityEngine;
using Zenject;

public class Finish : MonoBehaviour, ICollectible
{
    // Инъекции зависимостей
    [Inject] private IGameStateManager gameStateManager;
    [Inject] private IAudioManager audioManager;

    // "Сбор" финиша
    public void Collect()
    {
        gameStateManager.SetLevelCompleted();
        audioManager.PlaySound("LevelComplete");
    }
}