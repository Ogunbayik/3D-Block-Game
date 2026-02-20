using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class SpawnManager : MonoBehaviour
{
    private SignalBus _signalBus;

    private List<Shape> _activeShapes = new List<Shape>();

    private Queue<GameObject> _initialShapeQueue = new Queue<GameObject>();

    [Header("Initial Spawn Settings")]
    [SerializeField] private GameObject[] _initialShapePrefabs;
    [Header("General Spawn Settings")]
    [SerializeField] private GameObject[] _randomShapePrefabs;
    [SerializeField] private List<SpawnSlot> _spawnSlots = new List<SpawnSlot>();

    private int _rotationCount = 4;

    public Func<bool> OnAllShapesPlaced;
    public List<Shape> ActiveShapes => _activeShapes;

    [Inject]
    public void Construct(SignalBus signalBus) => _signalBus = signalBus;

    private void OnEnable()
    {
        _signalBus.Subscribe<GameSignal.OnShapePlaced>(RemoveShape);
        _signalBus.Subscribe<GameSignal.OnBoardGenerated>(InitialSpawn);
    }
    private void OnDisable()
    {
        _signalBus.Unsubscribe<GameSignal.OnShapePlaced>(RemoveShape);
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
    public void RemoveShape(GameSignal.OnShapePlaced signal)
    {
        _activeShapes.Remove(signal.Shape.GetComponent<Shape>());

        if (_activeShapes.Count == 0)
            _signalBus.Fire(new GameSignal.OnAllShapePlaced());

    }
    public void CheckAndSpawnNewShapes()
    {
        if (OnAllShapesPlaced())
            HandleSpawnBlocks();
    }
    private void HandleSpawnBlocks()
    {
        var spawnList = GetSpawnList();

        for (int i = 0; i < _spawnSlots.Count; i++)
        {
            var shape = Instantiate(spawnList[i], _spawnSlots[i].transform);
            var shapeScript = shape.GetComponent<Shape>();

            shape.transform.localPosition = Vector3.zero;
            shape.transform.rotation = GetRandomRotation();
            _activeShapes.Add(shapeScript);

            _spawnSlots[i].AssingnShape(shape);
        }

        _signalBus.Fire(new GameSignal.OnSpawnedNewBlocks());
    }
    private List<GameObject> GetSpawnList()
    {
        var spawnList = new List<GameObject>();

        if (_initialShapeQueue.Count > 0)
        {
            for (int i = 0; i < _spawnSlots.Count; i++)
            {
                var shape = _initialShapeQueue.Dequeue();
                spawnList.Add(shape);
            }
            Debug.Log($"Initial List: {_initialShapeQueue.Count}");
        }
        else
        {
            Debug.Log("Random Spawn ediyoruz.");
            for (int i = 0; i < _spawnSlots.Count; i++)
            {
                var shape = GetRandomShape();
                spawnList.Add(shape);
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
    private GameObject GetRandomShape()
    {
        var randomIndex = UnityEngine.Random.Range(0, _randomShapePrefabs.Length);
        var randomShape = _randomShapePrefabs[randomIndex];

        return randomShape;
    }
}
public enum ShapeRotation
{
    Deg0 = 0,
    Deg90 = 1,
    Deg180 = 2,
    Deg270 = 3
}
