using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Header("Game Managers")]
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private BoardManager _boardManager;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private GameManager _gameManager;
    public override void InstallBindings()
    {
        Container.Bind<SpawnManager>().FromInstance(_spawnManager).AsSingle();
        Container.Bind<BoardManager>().FromInstance(_boardManager).AsSingle();
        Container.Bind<LevelManager>().FromInstance(_levelManager).AsSingle();
        Container.Bind<GameManager>().FromInstance(_gameManager).AsSingle();
    }
}