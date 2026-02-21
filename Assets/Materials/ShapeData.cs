using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShapeData", menuName = "Scriptable Object/Shape Data")]
public class ShapeData : ScriptableObject
{
    [Header("Shape Settings")]
    public Color ShapeColor = Color.white;

    [Header("Block Coordinates")]
    public List<Vector2Int> BlockCoordinates;
}
