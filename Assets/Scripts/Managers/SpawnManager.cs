using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _blockPrefabs;
    [SerializeField] private List<SpawnSlot> _spawnPositionList = new List<SpawnSlot>();

    public Func<bool> OnAllShapesPlaced;
    private void Start()
    {
        SpawnRandomBlock();

        OnAllShapesPlaced = () => _spawnPositionList.TrueForAll(shape => shape.IsPlaced);
    }
    public void CheckAndSpawnNewShapes()
    {
        if (OnAllShapesPlaced())
            SpawnRandomBlock();
    }
    private void SpawnRandomBlock()
    {
        for (int i = 0; i < _spawnPositionList.Count; i++)
        {
            var randomShape = GetRandomShape();
            var shape = Instantiate(randomShape, _spawnPositionList[i].transform);
            shape.transform.localPosition = Vector3.zero;

            SpawnSlot spawnSlot = _spawnPositionList[i].GetComponent<SpawnSlot>();
            spawnSlot.AssingnShape(shape);
        }
    }
    private GameObject GetRandomShape()
    {
        var randomIndex = UnityEngine.Random.Range(0, _blockPrefabs.Length);
        var randomBlock = _blockPrefabs[randomIndex];

        return randomBlock;
    }

}
