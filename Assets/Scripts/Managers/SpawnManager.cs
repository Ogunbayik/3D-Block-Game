using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class SpawnManager : MonoBehaviour
{
    private SignalBus _signalBus;

    private BaseShape.Pool _shapePool;

    private List<BaseShape> _activeShapes = new List<BaseShape>();

    private Queue<GameObject> _initialShapeQueue = new Queue<GameObject>();

    [Header("Initial Spawn Settings")]
    [SerializeField] private GameObject[] _initialShapePrefabs;
    [Header("General Spawn Settings")]
    [SerializeField] private GameObject[] _randomShapePrefabs;
    [SerializeField] private List<SpawnSlot> _spawnSlots = new List<SpawnSlot>();

    private int _rotationCount = 4;

    public Func<bool> OnAllShapesPlaced;
    public List<BaseShape> ActiveShapes => _activeShapes;

    [SerializeField] private ShapeData _testData;

    [Inject]
    public void Construct(SignalBus signalBus,BaseShape.Pool shapePool)
    {
        _signalBus = signalBus;
        _shapePool = shapePool;
    }

    private void OnEnable()
    {
        _signalBus.Subscribe<GameSignal.OnShapePlaced>(HandleRemoveShape);
        _signalBus.Subscribe<GameSignal.OnBoardGenerated>(InitialSpawn);
    }
    private void OnDisable()
    {
        _signalBus.Unsubscribe<GameSignal.OnShapePlaced>(HandleRemoveShape);
        _signalBus.Unsubscribe<GameSignal.OnBoardGenerated>(InitialSpawn);
    }
    private void Start() => Initialize();
    private void Initialize()
    {
        AddQueueToInitial();
        OnAllShapesPlaced = () => _spawnSlots.TrueForAll(shape => shape.IsPlaced);
    }
    private void AddQueueToInitial()
    {
        if (_initialShapePrefabs.Length % _spawnSlots.Count != 0)
        {
            throw new System.InvalidOperationException(
                $"Change initial shape count.. The prefab number must be 0 modulo {_spawnSlots.Count}"
            );
        }

        for (int i = 0; i < _initialShapePrefabs.Length; i++)
            _initialShapeQueue.Enqueue(_initialShapePrefabs[i]);
    }
    private void InitialSpawn()
    {
        HandleSpawnBlocks();
    }
    public void HandleRemoveShape(GameSignal.OnShapePlaced signal)
    {
        ActiveShapes.Remove(signal.Shape);

        if (ActiveShapes.Count == 0)
            _signalBus.Fire(new GameSignal.OnAllShapePlaced());
    }
    public void CheckAndSpawnNewShapes()
    {
        if (OnAllShapesPlaced())
            HandleSpawnBlocks();
    }
    private void HandleSpawnBlocks()
    {
        var spawnShapeData = GetSpawnShapeData();

        for (int i = 0; i < _spawnSlots.Count; i++)
        {
            var shape = _shapePool.Spawn(_shapePool);
            shape.Setup(spawnShapeData[i]);
            shape.transform.position = _spawnSlots[i].transform.position;
            _activeShapes.Add(shape);

            _spawnSlots[i].AssingnShape(shape);
        }

        _signalBus.Fire(new GameSignal.OnSpawnedNewBlocks());
    }
    private List<ShapeData> GetSpawnShapeData()
    {
        var spawnList = new List<ShapeData>();

        if(_initialShapeQueue.Count > 0)
        {
            //Initial datalarý alýp onlarý oluþturacaðýz
        }
        else
        {
            //Random datayý alýp onlarý oluþturacaðýz.
            for (int i = 0; i < _spawnSlots.Count; i++)
            {
                spawnList.Add(_testData);
            }
        }

        return spawnList;
    }
    private Quaternion GetRandomRotation()
    {
        ShapeRotation randomEnum = (ShapeRotation)UnityEngine.Random.Range(0, _rotationCount);

        return GetRotationFromEnum(randomEnum);
    }
    public Quaternion GetRotationFromEnum(ShapeRotation rotationEnum)
    {
        switch (rotationEnum)
        {
            case ShapeRotation.Deg90: 
                return Quaternion.Euler(0f, 90f, 0f);
            case ShapeRotation.Deg180: 
                return Quaternion.Euler(0f, 180f, 0f);
            case ShapeRotation.Deg270: 
                return Quaternion.Euler(0f, 270f, 0f);
            case ShapeRotation.Deg0:
            default: 
                return Quaternion.identity;
        }
    }
}
public enum ShapeRotation
{
    Deg0 = 0,
    Deg90 = 1,
    Deg180 = 2,
    Deg270 = 3
}
