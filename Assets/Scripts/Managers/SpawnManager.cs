using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class SpawnManager : MonoBehaviour
{
    private SignalBus _signalBus;

    private List<Shape> _activeShapes = new List<Shape>();

    [Header("Initial Spawn Settings")]
    [SerializeField] private GameObject[] _initialShapePrefabs;
    [Header("General Spawn Settings")]
    [SerializeField] private GameObject[] _randomShapePrefabs;
    [SerializeField] private List<SpawnSlot> _spawnSlotList = new List<SpawnSlot>();

    private int _rotationCount = 4;

    public Func<bool> OnAllShapesPlaced;
    public List<Shape> ActiveShapes => _activeShapes;
    
    [Inject]
    public void Construct(SignalBus signalBus) => _signalBus = signalBus;

    private void OnEnable()
    {
        _signalBus.Subscribe<GameSignal.OnShapePlaced>(RemoveShape);
    }
    private void OnDisable()
    {
        _signalBus.Unsubscribe<GameSignal.OnShapePlaced>(RemoveShape);
    }
    private void Start() => Initialize();
    private void Initialize()
    {
        OnAllShapesPlaced = () => _spawnSlotList.TrueForAll(shape => shape.IsPlaced);
    }

    public void RemoveShape(GameSignal.OnShapePlaced signal) => _activeShapes.Remove(signal.Shape.GetComponent<Shape>());
    public void CheckAndSpawnNewShapes()
    {
        SpawnRandomBlock();
    }
    private void SpawnRandomBlock()
    {
        for (int i = 0; i < _spawnSlotList.Count; i++)
        {
            //TODO Rotasyon ve Pozisyonlarda düzenleme yapýlacak
            var randomShape = GetRandomShape();
            var shape = Instantiate(randomShape, _spawnSlotList[i].transform);
            var shapeScript = shape.GetComponent<Shape>();
            shape.transform.localPosition = Vector3.zero;
            shape.transform.rotation = GetRandomRotation();
            _activeShapes.Add(shapeScript);

            SpawnSlot spawnSlot = _spawnSlotList[i].GetComponent<SpawnSlot>();
            spawnSlot.AssingnShape(shape);
        }

        _signalBus.Fire(new GameSignal.OnSpawnedNewBlocks());
    }

    private Quaternion GetRandomRotation()
    {
        float index = UnityEngine.Random.Range(0, _rotationCount);
        float degree = GameConstant.GameSetting.ROUND_ANGLE / _rotationCount;

        Quaternion rotation = Quaternion.Euler(0f, degree * index, 0f);
        return rotation;
    }
    private GameObject GetRandomShape()
    {
        var randomIndex = UnityEngine.Random.Range(0, _randomShapePrefabs.Length);
        var randomShape = _randomShapePrefabs[randomIndex];

        return randomShape;
    }

}
