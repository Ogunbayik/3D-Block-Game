using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node 
{
    public bool isOccupied;

    public BlockColor color;

    public GameObject storedBlockObject;

    public Node(GameObject storedObject)
    {
        color = BlockColor.None;
        isOccupied = false;
        storedBlockObject = storedObject;
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
