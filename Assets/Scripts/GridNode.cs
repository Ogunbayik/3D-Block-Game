using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GridNode : MonoBehaviour
{
    [Header("Visual References")]
    [SerializeField] private GameObject _baseVisual;
    [SerializeField] private Block _block;

    private bool _isOccupied;
    public bool IsOccupied => _isOccupied;
    public Block Block => _block;
    private void Awake() => ToggleBlock(false);
    public void Clear()
    {
        _isOccupied = false;
        ToggleBlock(false);
    }
    public void SetOccupiedStatus(bool isActive) => _isOccupied = isActive;
    public void ToggleBlock(bool isActive) => _block.gameObject.SetActive(isActive);
    public void SetBaseScale(Vector3 scale) => _baseVisual.transform.localScale = scale;

    public class Factory : PlaceholderFactory<GridNode> { }
}
