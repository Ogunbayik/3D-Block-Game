using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Header("Game Managers")]
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private BoardManager _boardManager;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private GameManager _gameManager;
    [Header("Factory Settings")]
    [SerializeField] private GameObject _testGridNode;
    [SerializeField] private Transform _gridGroup;
    [Header("Pool Settings")]
    [SerializeField] private GameObject _shapePrefab;
    [SerializeField] private Transform _shapeGroup;
    public override void InstallBindings()
    {
        Container.Bind<SpawnManager>().FromInstance(_spawnManager).AsSingle();
        Container.Bind<BoardManager>().FromInstance(_boardManager).AsSingle();
        Container.Bind<LevelManager>().FromInstance(_levelManager).AsSingle();
        Container.Bind<GameManager>().FromInstance(_gameManager).AsSingle();
        Container.BindInterfacesAndSelfTo<GameFlowManager>().AsSingle().NonLazy();

        Container.BindFactory<GridNode, GridNode.Factory>()
           .FromComponentInNewPrefab(_testGridNode)
           .UnderTransform(_gridGroup)
           .AsSingle();

        Container.BindMemoryPool<BaseShape, BaseShape.Pool>()
            .FromComponentInNewPrefab(_shapePrefab)
            .UnderTransform(_shapeGroup)
            .AsCached();
    }
}