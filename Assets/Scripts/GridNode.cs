using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GridNode : MonoBehaviour
{
    [Header("Visual References")]
    [SerializeField] private GameObject _baseVisual;
    [SerializeField] private GameObject _blockVisual;

    private bool _isOccupied;
    public bool IsOccupied => _isOccupied;
    private void Awake() => ToggleBlock(false);
    public void PlaceBlock()
    {
        _isOccupied = true;
        ToggleBlock(true);
    }
    public void Clear()
    {
        _isOccupied = false;
        ToggleBlock(false);
    }
    private void ToggleBlock(bool isActive) => _blockVisual.SetActive(isActive);
    public void SetBaseScale(Vector3 scale) => _baseVisual.transform.localScale = scale;

    public class Factory : PlaceholderFactory<GridNode> { }
}
