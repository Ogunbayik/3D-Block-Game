using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Header("Visual References")]
    [SerializeField] private GameObject _cubePrefab;
    [Header("Board Settings")]
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private float _cellScale;
    [Space]
    [Header("Test Settings")]
    [SerializeField] private Color[] _testColors;

    private Node[,] _allNodes;
    void Start()
    {
        _allNodes = new Node[_width, _height];

        for (int i = 0; i < _width; i++)
        {
            for(int j = 0; j < _height; j++)
            {
                var cellObj = Instantiate(_cubePrefab);
                var cell = cellObj.GetComponent<Cell>();
                var spawnPosition = new Vector3(i, 0f, j);

                cell.SetCellColor(GetRandomColor());
                cellObj.name = $"Cell[{i},{j}]";
                cellObj.transform.position = spawnPosition;
                cellObj.transform.localScale = new Vector3(_cellScale, _cellScale, _cellScale);
                _allNodes[i, j] = new Node(cellObj);
            }
        }
    }

    private Color GetRandomColor()
    {
        var randomIndex = Random.Range(0, _testColors.Length);
        var randomColor = _testColors[randomIndex];

        return randomColor;
    }
}
