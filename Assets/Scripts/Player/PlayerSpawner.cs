using UnityEngine;
using Zenject;

public class PlayerSpawner : IPlayerSpawner
{
    // Префаб игрока
    private readonly GameObject playerPrefab;
    // Провайдер стартовой позиции
    private readonly IStartPositionProvider startPositionProvider;
    // Контейнер DI
    private readonly DiContainer container;
    // Следование камеры
    private readonly CameraFollow cameraFollow;

    private GameObject currentPlayer;

    // Конструктор с инъекцией
    [Inject]
    public PlayerSpawner(
        [Inject(Id = "PlayerPrefab")] GameObject playerPrefab,
        IStartPositionProvider startPositionProvider,
        DiContainer container,
        CameraFollow cameraFollow)
    {
        this.playerPrefab = playerPrefab;
        this.startPositionProvider = startPositionProvider;
        this.container = container;
        this.cameraFollow = cameraFollow;
    }

    // Спавн игрока
    public void SpawnPlayer(int levelIndex)
    {
        Transform startPos = startPositionProvider.GetStartPosition(levelIndex);
        if (startPos == null)
        {
            Debug.LogWarning($"Стартовой позиции нет для уровня {levelIndex}");
            return;
        }

        // Уничтожение существующего игрока
        if (currentPlayer != null)
        {
            GameObject.Destroy(currentPlayer);
            currentPlayer = null;
        }

        // Инстанцирование нового игрока
        GameObject player = container.InstantiatePrefab(playerPrefab, startPos.position, Quaternion.identity, null);
        currentPlayer = player;
        cameraFollow.SetTarget(player.transform);
    }
}