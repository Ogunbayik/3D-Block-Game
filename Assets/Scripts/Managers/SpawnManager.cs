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
    private Queue<ShapeData> _initialShapeQueue = new Queue<ShapeData>();

    [Header("Initial Spawn Settings")]
    [SerializeField] private List<ShapeData> _initialShapeData = new List<ShapeData>();
    [Header("General Spawn Settings")]
    [SerializeField] private List<Transform> _spawnPositions = new List<Transform>();
    [SerializeField] private List<ShapeData> _shapeDatas = new List<ShapeData>();

    public Func<bool> OnAllShapesPlaced;
    public List<BaseShape> ActiveShapes => _activeShapes;

    [Inject]
    public void Construct(SignalBus signalBus,BaseShape.Pool shapePool)
    {
        _signalBus = signalBus;
        _shapePool = shapePool;
    }
    private void Awake() => Initialize();
    private void Initialize()
    {
        if(_initialShapeData.Count > 0)
        {
            foreach (var shapeData in _initialShapeData)
                _initialShapeQueue.Enqueue(shapeData);
        }
    }
    public void RemoveActiveShape(BaseShape shape) => _activeShapes.Remove(shape);
    public void CheckAndSpawnNewShapes()
    {
        if (OnAllShapesPlaced())
            SpawnNewShape();
    }
    public void SpawnNewShape()
    {
        var spawnShapeData = GetSpawnShapeData();

        for (int i = 0; i < _spawnPositions.Count; i++)
        {
            var newShape = _shapePool.Spawn(_shapePool);
            newShape.Setup(spawnShapeData[i]);
            newShape.transform.position = _spawnPositions[i].position;
            _activeShapes.Add(newShape);
        }
    }
    private List<ShapeData> GetSpawnShapeData()
    {
        var spawnDataList = new List<ShapeData>();

        for (int i = 0; i < _spawnPositions.Count; i++)
        {
            if(_initialShapeQueue.Count > 0)
            {
                spawnDataList.Add(_initialShapeQueue.Dequeue());
            }
            else
            {
                spawnDataList.Add(GetRandomShapeData());
            }
        }

        return spawnDataList;
    }
    private ShapeData GetRandomShapeData()
    {
        var randomIndex = UnityEngine.Random.Range(0, _shapeDatas.Count);
        ShapeData shapeData = _shapeDatas[randomIndex];

        return shapeData;
    }
}
