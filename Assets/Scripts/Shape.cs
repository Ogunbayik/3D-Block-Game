using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Shape : MonoBehaviour, IPoolable<IMemoryPool>
{
    private IMemoryPool _pool;

    public List<Vector2Int> BlockOffsets { get; private set; } = new List<Vector2Int>();
    public void OnSpawned(IMemoryPool pool)
    {
        _pool = pool;
        InitializeShape();
    }
    public void OnDespawned()
    {
        _pool.Despawn(this);
    }
    private void Awake() => InitializeShape();
    public void InitializeShape()
    {
        BlockOffsets.Clear();

        foreach (Transform child in transform)
        {
            if (child.GetComponent<Block>() == null) continue;

            int x = Mathf.RoundToInt(child.localPosition.x);
            int z = Mathf.RoundToInt(child.localPosition.z);

            BlockOffsets.Add(new Vector2Int(x, z));
        }
    }

}
