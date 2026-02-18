using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class SpawnManager : MonoBehaviour
{
    private SignalBus _signalBus;

    private List<Shape> _activeShapes = new List<Shape>();

    [Header("Spawn Settings")]
    [SerializeField] private GameObject[] _blockPrefabs;
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
    private void Start()
    {
        SpawnRandomBlock();

        OnAllShapesPlaced = () => _spawnSlotList.TrueForAll(shape => shape.IsPlaced);
    }
    public void RemoveShape(GameSignal.OnShapePlaced signal) => _activeShapes.Remove(signal.Shape.GetComponent<Shape>());
    public void CheckAndSpawnNewShapes()
    {
        if (OnAllShapesPlaced())
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
        var randomIndex = UnityEngine.Random.Range(0, _blockPrefabs.Length);
        var randomBlock = _blockPrefabs[randomIndex];

        return randomBlock;
    }

}
