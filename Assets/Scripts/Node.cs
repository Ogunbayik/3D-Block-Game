using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node 
{
    public bool IsOccupied;

    public BlockColor Color;

    public GameObject StoredBlockObject;

    public Transform CellTransform;

    public Node(GameObject storedObject, Transform cellTransform)
    {
        Color = BlockColor.None;
        IsOccupied = false;
        StoredBlockObject = storedObject;
        CellTransform = cellTransform;
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
