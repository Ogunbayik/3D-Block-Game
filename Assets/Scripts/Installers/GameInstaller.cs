using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Header("Game Managers")]
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private BoardManager _boardManager;
    public override void InstallBindings()
    {
        Container.Bind<SpawnManager>().FromInstance(_spawnManager).AsSingle();
        Container.Bind<BoardManager>().FromInstance(_boardManager).AsSingle();
    }
}