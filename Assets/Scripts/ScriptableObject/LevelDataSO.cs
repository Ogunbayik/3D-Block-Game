using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Data", menuName = "Scriptable Object/Level Data")]
public class LevelDataSO : ScriptableObject
{
    [Header("Initial Shapes")]
    public List<PrePlacedShapeData> StartingShapes;
    [Header("Level Settings")]
    public int ID;
    public int Width;
    public int Height;
    public int PassScore;
}

[System.Serializable]
public struct PrePlacedShapeData
{
    public GameObject ShapePrefab;
    public Vector2Int StartCoordinate;

    public ShapeRotation Rotation;

}