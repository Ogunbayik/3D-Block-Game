using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node 
{
    public BlockColor Color;

    public Transform CellTransform;

    private bool _isOccupied;

    private GameObject _storedBlockObject;

    public GameObject StoredBlockObject => _storedBlockObject;
    public bool IsOccupied => _isOccupied;
    public Node(Transform cellTransform)
    {
        Color = BlockColor.None;
        _isOccupied = false;
        CellTransform = cellTransform;
    }
    public void PlaceBlock(GameObject obj)
    {
        _storedBlockObject = obj;
        _isOccupied = true;
    }
    public void Clear()
    {
        _storedBlockObject = null;
        _isOccupied = false;
    }
}

public enum BlockColor
{
    None,
    Red,
    Green,
    Blue,
    Yellow,
    Purple,
    Aqua
}
