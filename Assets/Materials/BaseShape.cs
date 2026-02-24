using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BaseShape : MonoBehaviour, IPoolable<IMemoryPool>
{
    private IMemoryPool _pool;

    [Header("References")]
    [SerializeField] private List<Block> _allChildBlocks;
    [SerializeField] private List<Block> _activeBlocks = new List<Block>();

    public ShapeData Data { get; private set; }

    public List<Block> ActiveBlocks => _activeBlocks;
    public void OnSpawned(IMemoryPool pool)
    {
        _pool = pool;
    }
    public void OnDespawned()
    {

    }
    public void ReturnToPool()
    {
        _activeBlocks.Clear();
        _pool.Despawn(this);
    }
    public void Setup(ShapeData data)
    {
        Data = data;

        foreach (var block in _allChildBlocks)
        {
            block.gameObject.SetActive(false);
        }

        foreach (Vector2Int coordinate in data.BlockCoordinates)
        {
            Block targetBlock = _allChildBlocks.Find(block => block.LocalX == coordinate.x && block.LocalY == coordinate.y);

            if (targetBlock != null)
            {
                targetBlock.gameObject.SetActive(true);
                targetBlock.SetColor(data.ShapeColor);

                _activeBlocks.Add(targetBlock);
            }
            else
            {
                Debug.LogWarning($"ShapeData ({coordinate.x}, {coordinate.y}) koordinatýný istedi ama Prefab'da böyle bir BlockVisual yok!");
            }
        }
    }

    public class Pool : MonoPoolableMemoryPool<IMemoryPool,BaseShape> { }
}
