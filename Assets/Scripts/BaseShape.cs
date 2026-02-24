using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BaseShape : MonoBehaviour, IPoolable<IMemoryPool>
{
    private ShapeAnimationController controller;

    public event Action OnClicked;
    public event Action OnPlacedStarted;
    public event Action OnReleased;

    private IMemoryPool _pool;

    [Header("References")]
    [SerializeField] private List<Block> _allChildBlocks;
    [SerializeField] private List<Block> _activeBlocks = new List<Block>();
    [Header("Layer Settings")]
    [SerializeField] private int _draggableLayer;
    [SerializeField] private int _releasedLayer;

    private Vector3 _spawnPosition;
    public List<Block> ActiveBlocks => _activeBlocks;
    public ShapeData Data { get; private set; }
    public Vector3 SpawnPosition => _spawnPosition;

    public int DraggableLayer => _draggableLayer;
    public int ReleasedLayer => _releasedLayer;
    private void Awake()
    {
        controller = GetComponent<ShapeAnimationController>();
    }
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
    public void SetSpawnPosition(Vector3 position) => _spawnPosition = position;
    public void ChangeLayerOfAllBlocks(int targetLayerID)
    {
        foreach (var block in _activeBlocks)
            block.gameObject.layer = targetLayerID;
    }
    public void Click() => OnClicked?.Invoke();
    public void StartPlacing() => OnPlacedStarted?.Invoke();
    public void Released() => OnReleased?.Invoke();

    public class Pool : MonoPoolableMemoryPool<IMemoryPool,BaseShape> { }
}
