using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    // Префаб игрока
    [SerializeField] private GameObject playerPrefab;
    // Камера
    [SerializeField] private CameraFollow cameraFollow;

    public override void InstallBindings()
    {
        // Привязка LevelManager к ILevelManager
        Container.Bind<ILevelManager>().To<LevelManager>().FromComponentInHierarchy().AsSingle();

        // Привязка CameraBoundariesProvider к ICameraBoundariesProvider
        Container.Bind<ICameraBoundariesProvider>().To<CameraBoundariesProvider>().FromComponentInHierarchy().AsSingle();

        // Привязка StartPositionProvider к IStartPositionProvider
        Container.Bind<IStartPositionProvider>().To<StartPositionProvider>().FromComponentInHierarchy().AsSingle();

        // Привязка IEnemy к PatrolEnemy
        Container.Bind<IEnemy>().To<PatrolEnemy>().FromComponentsInHierarchy().AsTransient();

        // Привязка GameStarter
        Container.Bind<IGameStarter>().To<GameStarter>().AsSingle();

        // Привязка PlayerHealth
        Container.Bind<IPlayerHealth>().To<PlayerHealth>().AsSingle();

        // Привязка префаба игрока и камеры
        Container.BindInstance(playerPrefab).WithId("PlayerPrefab");
        Container.BindInstance(cameraFollow).AsSingle();

        // Привязка HealthUI
        Container.Bind<HealthUI>().FromComponentInHierarchy().AsSingle();

        // Привязка UIManager и интерфейсов
        Container.BindInterfacesAndSelfTo<UIManager>().FromComponentInHierarchy().AsSingle();

        // Привязка GameStateManager и интерфейсов
        Container.BindInterfacesAndSelfTo<GameStateManager>().FromComponentInHierarchy().AsSingle();

        // Привязка GameOverPanel
        Container.Bind<GameOverPanel>().FromComponentInHierarchy().AsSingle();

        // Привязка CoinManager как singleton
        Container.Bind<ICoinManager>().To<CoinManager>().AsSingle();

        // Привязка CoinsUI
        Container.Bind<CoinsUI>().FromComponentInHierarchy().AsSingle();

        // Привязка CoinRegistry
        Container.Bind<ICoinRegistry>().To<CoinRegistry>().AsSingle();

        // Привязка ResetManager
        Container.Bind<IResetManager>().To<ResetManager>().AsSingle();

        // Привязка AudioManager
        Container.Bind<IAudioManager>().To<AudioManager>().FromComponentInHierarchy().AsSingle();

        // Привязки для компонентов GameStarter
        Container.Bind<IPlayerSpawner>().To<PlayerSpawner>().AsSingle();
        Container.Bind<ILevelInitializer>().To<LevelInitializer>().AsSingle();
        Container.Bind<IResetOrchestrator>().To<ResetOrchestrator>().AsSingle();
    }
}