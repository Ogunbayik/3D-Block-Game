using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BoardManager : MonoBehaviour
{
    private SignalBus _signalBus;

    [Header("Visual References")]
    [SerializeField] private GameObject _cubePrefab;
    [Header("Board Settings")]
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private float _cellScale;
    [SerializeField] private float _cellScaleY;
    [Space]
    [Header("Test Settings")]
    [SerializeField] private Color[] _testColors;

    private HashSet<GameObject> _matchHash = new HashSet<GameObject>();
    private List<GameObject> _testList = new List<GameObject>();

    private Node[,] _allNodes;

    [Inject]
    public void Construct(SignalBus signalBus) => _signalBus = signalBus;
    void Start() => SetupBoard();
    private void SetupBoard()
    {
        _allNodes = new Node[_width, _height];

        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                var cellObj = Instantiate(_cubePrefab);
                var cell = cellObj.GetComponent<Cell>();
                var spawnPosition = new Vector3(x, 0f, z);

                cell.SetCellColor(GetRandomColor());
                cell.SetCellScale(new Vector3(_cellScale, _cellScaleY, _cellScale));

                cellObj.name = $"Cell[{x},{z}]";
                cellObj.transform.position = spawnPosition;
                _allNodes[x, z] = new Node(cellObj.transform);
            }
        }
    }
    private void OnEnable() => _signalBus.Subscribe<GameSignal.OnShapePlaced>(ProcessMatches);
    private void OnDisable() => _signalBus.Unsubscribe<GameSignal.OnShapePlaced>(ProcessMatches);
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
            var childPosition = WorldToGridPosition(child.position);
            Debug.Log($"X: {childPosition.x}, Z: {childPosition.y}");

            //Alanýn içerisinde mi
            if (childPosition.x >= _width || childPosition.x < 0 || childPosition.y >= _height || childPosition.y < 0)
                return false;

            //Node'lar uygun mu
            if (_allNodes[childPosition.x, childPosition.y].IsOccupied)
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

            targetNode.PlaceBlock(child.gameObject);

            child.position = new Vector3(position.x, 0f, position.y);

            child.GetComponent<BlockItem>().SetNode(targetNode);
            child.SetParent(targetNode.CellTransform);
            child.gameObject.layer = LayerMask.NameToLayer("Placed");
        }

        Destroy(shapeObj);

        return true;
    }
    private void ProcessMatches()
    {   
        //Yatay objeler eklendi.
        var fullRowIndices = GetFullRowIndices();

        foreach(var rowIndex in fullRowIndices)
        {
            for (int x = 0; x < _width; x++)
            {
                var block = _allNodes[x, rowIndex].StoredBlockObject;

                if (block != null)
                {
                    _matchHash.Add(block);

                    if(!_testList.Contains(block))
                        _testList.Add(block);
                }
            }
        }

        //Dikey objeler eklendi.
        var fullColIndices = GetFullColIndices();

        foreach (var colIndex in fullColIndices)
        {
            for (int z = 0; z < _height; z++)
            {
                var block = _allNodes[colIndex, z].StoredBlockObject;

                if (block != null)
                {
                    _matchHash.Add(block);

                    if (!_testList.Contains(block))
                        _testList.Add(block);
                }
            }
        }

        if (_matchHash.Count > 0)
        {
            RemoveBlocks(_matchHash);

            _signalBus.Fire(new GameSignal.OnMatchesFound());

            _matchHash.Clear();
        }
    }
    private void RemoveBlocks(HashSet<GameObject> blocksToDestroy)
    {
        foreach (var block in blocksToDestroy)
        {
            if (block == null)
                continue;

            BlockItem blockItem = block.GetComponent<BlockItem>();

            if (blockItem != null && blockItem.CurrentNode != null)
                blockItem.CurrentNode.Clear();

            Destroy(block);
        }
    }
    private List<int> GetFullRowIndices()
    {
        List<int> fullRowIndices = new List<int>();

        for (int z = 0; z < _height; z++)
        {
            bool isRowMatch = true;

            for (int x = 0; x < _width; x++)
            {
                if (!_allNodes[x,z].IsOccupied)
                {
                    isRowMatch = false;
                    break;
                }
            }

            if (isRowMatch)
                fullRowIndices.Add(z);
        }

        return fullRowIndices;
    }
    private List<int> GetFullColIndices()
    {
        List<int> fullColIndices = new List<int>();

        for (int x = 0; x < _width; x++)
        {
            bool isColMatch = true;

            for (int z = 0; z < _height; z++)
            {
                if (!_allNodes[x, z].IsOccupied)
                {
                    isColMatch = false;
                    break;
                }
            }

            if (isColMatch)
                fullColIndices.Add(x);
        }

        return fullColIndices;
    }

    private void DestroyMatchBlocks()
    {
        foreach (var block in _matchHash)
        {
            _matchHash.Remove(block);
            Destroy(block);

            _matchHash.Clear();
        }
    }
    public Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        //Deðerleri yuvarlýyoruz.
        int x = Mathf.RoundToInt(worldPosition.x);
        int y = Mathf.RoundToInt(worldPosition.z);

        return new Vector2Int(x, y);
    }
}
