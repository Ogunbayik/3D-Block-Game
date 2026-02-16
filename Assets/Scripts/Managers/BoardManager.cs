using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Header("Visual References")]
    [SerializeField] private GameObject _cubePrefab;
    [Header("Board Settings")]
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private float _cellScale;
    [SerializeField] private float _offsetY;
    [Space]
    [Header("Test Settings")]
    [SerializeField] private Color[] _testColors;

    private Node[,] _allNodes;
    void Start() => SetupBoard();
    private void SetupBoard()
    {
        _allNodes = new Node[_width, _height];

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                var cellObj = Instantiate(_cubePrefab);
                var cell = cellObj.GetComponent<Cell>();
                var spawnPosition = new Vector3(i, 0f, j);

                cell.SetCellColor(GetRandomColor());
                cell.SetCellScale(new Vector3(_cellScale, _cellScale, _cellScale));

                cellObj.name = $"Cell[{i},{j}]";
                cellObj.transform.position = spawnPosition;
                _allNodes[i, j] = new Node(cellObj, cellObj.transform);
            }
        }
    }
    //TEST PART
    private Color GetRandomColor()
    {
        var randomIndex = Random.Range(0, _testColors.Length);
        var randomColor = _testColors[randomIndex];

        return randomColor;
    }
    public bool CheckIfShapeFits(GameObject block)
    {
        //Yerleþtirilebilecek obje kontrolü yapýyoruz.
        foreach(Transform child in block.transform)
        {
            var chilcPosition = WorldToGridPosition(child.position);
            Debug.Log($"X: {chilcPosition.x}, Z: {chilcPosition.y}");

            //Alanýn içerisinde mi
            if (chilcPosition.x >= _width || chilcPosition.x < 0 || chilcPosition.y >= _height || chilcPosition.y < 0)
                return false;

            //Node'lar uygun mu
            if (_allNodes[chilcPosition.x, chilcPosition.y].IsOccupied)
                return false;
        }

        return true;
    }
    public bool TryPlaceShape(GameObject shapeObj)
    {
        if (!CheckIfShapeFits(shapeObj))
            return false;

        List<Transform> shapeChildren = new List<Transform>();
        foreach (Transform shapeChild in shapeObj.transform) shapeChildren.Add(shapeChild);

        foreach (var child in shapeChildren)
        {
            var position = WorldToGridPosition(child.position);
            Node targetNode = _allNodes[position.x, position.y];

            targetNode.IsOccupied = true;
            targetNode.StoredBlockObject = child.gameObject;

            child.position = new Vector3(position.x, _offsetY, position.y);

            child.SetParent(targetNode.CellTransform);
            child.gameObject.layer = LayerMask.NameToLayer("Placed");
        }

        Destroy(shapeObj);

        //Yatay ve Dikey Kontrol yapacaðýz.

        return true;
    }
    public Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        //Deðerleri yuvarlýyoruz.
        int x = Mathf.RoundToInt(worldPosition.x);
        int y = Mathf.RoundToInt(worldPosition.z);

        return new Vector2Int(x, y);
    }
}
