using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BoardManager : MonoBehaviour
{
    private SignalBus _signalBus;

    private GridNode.Factory _gridNodeFactory;

    private SpawnManager _spawnManager;
    private LevelManager _levelManager;

    [Header("Visual References")]
    [SerializeField] private GameObject _cubePrefab;
    [Header("Board Settings")]
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private float _gridScale;
    [SerializeField] private float _gridScaleY;
    [Space]
    [Header("Test Settings")]
    [SerializeField] private Color[] _testColors;

    private HashSet<GridNode> _gridNodeHash = new HashSet<GridNode>();

    private GridNode[,] _allGridNodes;

    [Inject]
    public void Construct(GridNode.Factory gridNodeFactory,SignalBus signalBus, SpawnManager spawnManager,LevelManager levelManager)
    {
        _gridNodeFactory = gridNodeFactory;
        _signalBus = signalBus;
        _spawnManager = spawnManager;
        _levelManager = levelManager;
    }
    private void HandleGameOver()
    {
        //TODO Board Daðýlma animasyonu eklenecek
    }
    public void GeneratedBoard()
    {
        var currentLevel = _levelManager.CurrentLevel;

        _allGridNodes = new GridNode[currentLevel.Width, currentLevel.Height];

        for (int x = 0; x < currentLevel.Width; x++)
        {
            for (int z = 0; z < currentLevel.Height; z++)
            {
                var gridNode = _gridNodeFactory.Create();
                var gridPosition = new Vector3(x, 0f, z);
                var gridScale = new Vector3(_gridScale,_gridScaleY, _gridScale);

                gridNode.transform.position = gridPosition;
                gridNode.SetBaseScale(gridScale);
                gridNode.name = $"Grid[{x},{z}]";
                _allGridNodes[x,z] = gridNode;
            }
        }
    }
    public void SetupLevel()
    {
        var currentLevel = _levelManager.CurrentLevel;

        foreach (var startingShape in currentLevel.StartingShapes)
        {
            PlaceInitialShape(startingShape.ShapeData, startingShape.StartCoordinate);
        }
    }
    public void PlaceInitialShape(ShapeData shapeData, Vector2Int startCoordinate)
    {
        foreach (Vector2Int offset in shapeData.BlockCoordinates)
        {
            int targetX = startCoordinate.x + offset.x;
            int targetZ = startCoordinate.y + offset.y;

            if (targetX >= 0 && targetX < _width && targetZ >= 0 && targetZ < _height)
            {
                GridNode targetNode = _allGridNodes[targetX, targetZ];

                // Eðer o hücre zaten dolu deðilse, bloðu yerleþtir!
                if (!targetNode.IsOccupied)
                {
                    targetNode.SetOccupiedStatus(true);
                    targetNode.ToggleBlock(true);
                    // Ýsteðe baðlý: Eðer hücreye ShapeData'nýn rengini veya "Bu bir engeldir" 
                    // bilgisini vermek istersen burada targetNode.SetVisual(shapeData.Color) diyebilirsin.
                }
            }
        }
    }
    public bool CheckIfShapeFits(BaseShape selectedShape)
    {
        var currentLevel = _levelManager.CurrentLevel;

        foreach (var block in selectedShape.ActiveBlocks)
        {
            var blockPosition = WorldToGridPosition(block.transform.position);

            if (blockPosition.x >= currentLevel.Width || blockPosition.x < 0 || blockPosition.y >= currentLevel.Height || blockPosition.y < 0)
                return false;

            if (_allGridNodes[blockPosition.x, blockPosition.y].IsOccupied)
                return false;
        }

        return true;
    }
    public bool TryPlaceShape(BaseShape selectedShape)
    {
        if (!CheckIfShapeFits(selectedShape))
            return false;

        HandlePlaceSequence(selectedShape).Forget();

        return true;
    }
    private async UniTask HandlePlaceSequence(BaseShape selectedShape)
    {
        float jellyDuration = 0.6f;
        Sequence placeSequence = DOTween.Sequence();

        //Ýlk olarak Node Occupied edilecek.
        SetNodeOccupied(selectedShape, true);

        var targetPosition = WorldToGridPosition(selectedShape.transform.position);
        selectedShape.transform.position = new Vector3(targetPosition.x, 0f, targetPosition.y);

        ShapeAnimationController shapeAnimation = selectedShape.GetComponent<ShapeAnimationController>();

        placeSequence.AppendCallback(() => shapeAnimation.PlayJellySequence());
        placeSequence.AppendInterval(jellyDuration);
        placeSequence.AppendCallback(() => ToggleNodeBlock(selectedShape, true));
        placeSequence.JoinCallback(() => selectedShape.ReturnToPool());
    }
    private void SetNodeOccupied(BaseShape selectedShape, bool isActive)
    {
        var shapeblocks = selectedShape.ActiveBlocks;
        foreach (var block in shapeblocks)
        {
            var blockPosition = WorldToGridPosition(block.transform.position);
            GridNode targetNode = _allGridNodes[blockPosition.x, blockPosition.y];

            targetNode.SetOccupiedStatus(isActive);
        }
    }
    private void ToggleNodeBlock(BaseShape selectedShape, bool isActive)
    {
        var shapeblocks = selectedShape.ActiveBlocks;
        foreach (var block in shapeblocks)
        {
            var blockPosition = WorldToGridPosition(block.transform.position);
            GridNode targetNode = _allGridNodes[blockPosition.x, blockPosition.y];

            targetNode.ToggleBlock(isActive);
        }
    }
    public void ProcessMatches()
    {
        //Yatay objeler eklendi.
        CollectHorizontalMatch();
        //Dikey objeler eklendi.
        CollectVerticalMatch();

        if (_gridNodeHash.Count > 0)
        {
            var rowMatchCount = GetFullRowIndices().Count;
            var colMatchCount = GetFullColIndices().Count;

            _signalBus.Fire(new GameSignal.OnMatchesFound(rowMatchCount,colMatchCount));

            RemoveBlocks(_gridNodeHash);

            _gridNodeHash.Clear();
        }
    }
    private void CollectHorizontalMatch()
    {
        var fullRowIndices = GetFullRowIndices();
        var currentLevel = _levelManager.CurrentLevel;

        foreach (var rowIndex in fullRowIndices)
        {
            for (int x = 0; x < currentLevel.Width; x++)
            {
                var gridNode = _allGridNodes[x, rowIndex];

                if (gridNode != null)
                    _gridNodeHash.Add(gridNode);
            }
        }
    }
    private void CollectVerticalMatch()
    {
        var fullColIndices = GetFullColIndices();
        var currentLevel = _levelManager.CurrentLevel;

        foreach (var colIndex in fullColIndices)
        {
            for (int z = 0; z < currentLevel.Height; z++)
            {
                var gridNode = _allGridNodes[colIndex, z];

                if (gridNode != null)
                    _gridNodeHash.Add(gridNode);
            }
        }
    }
    public void CheckGameOver()
    {
        var currentShapes = _spawnManager.ActiveShapes;
        if (currentShapes.Count == 0) return;

        bool canMove = false;
        foreach (var shape in currentShapes)
        {
            if (HasValidPlacement(shape.Data.BlockCoordinates))
            {
                canMove = true;
                break;
            }
        }

        if (!canMove)
        {
            Debug.Log("Dont any move");
            _signalBus.Fire(new GameSignal.OnGameStateChanged(GameState.GameOver));
        }
    }
    public bool HasValidPlacement(List<Vector2Int> blockOffsets)
    {
        var currentLevel = _levelManager.CurrentLevel;
        for (int x = 0; x < currentLevel.Width; x++)
        {
            for (int z = 0; z < currentLevel.Height; z++)
            {
                if (TryFitAt(blockOffsets, x, z))
                {
                    Debug.Log($"StartPosition X:{x}, Z: {z}");
                    return true;
                }
            }
        }
        return false;
    }
    private bool TryFitAt(List<Vector2Int> blockOffsets, int startX, int startZ)
    {
        var currentLevel = _levelManager.CurrentLevel;

        foreach (Vector2Int offset in blockOffsets)
        {
            int targetX = startX + offset.x;
            int targetZ = startZ + offset.y;

            if (targetX >= currentLevel.Width || targetZ >= currentLevel.Height || targetX < 0 || targetZ < 0)
                return false;

            if (_allGridNodes[targetX, targetZ].IsOccupied)
                return false;
        }
        return true;
    }
    private void RemoveBlocks(HashSet<GridNode> gridToMatch)
    {
        foreach(var gridNode in gridToMatch)
        {
            gridNode.Clear();
        }
    }
    private List<int> GetFullRowIndices()
    {
        List<int> fullRowIndices = new List<int>();
        var currentLevel = _levelManager.CurrentLevel;

        for (int z = 0; z < currentLevel.Height; z++)
        {
            bool isRowMatch = true;

            for (int x = 0; x < currentLevel.Width; x++)
            {
                if (!_allGridNodes[x,z].IsOccupied)
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
        var currentLevel = _levelManager.CurrentLevel;

        for (int x = 0; x < currentLevel.Width; x++)
        {
            bool isColMatch = true;

            for (int z = 0; z < currentLevel.Height; z++)
            {
                if (!_allGridNodes[x, z].IsOccupied)
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
    public Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        //Deðerleri yuvarlýyoruz.
        int x = Mathf.RoundToInt(worldPosition.x);
        int z = Mathf.RoundToInt(worldPosition.z);

        return new Vector2Int(x, z);
    }
}
