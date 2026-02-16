using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public BoardManager _boardManager;

    [Header("Raycast Settings")]
    [SerializeField] private float _rayDistance;
    [SerializeField] private float _offsetY;
    [Header("Layer Settings")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _draggableLayer;

    private GameObject _selectedblock;

    private Vector3 _offset;
    private Vector3 _selectPosition;

    private Camera _mainCamera;
    private void Start() => Initialize();
    private void Initialize() => _mainCamera = Camera.main;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            HandleSelectBlock();

        if (Input.GetMouseButton(0) && _selectedblock != null)
            HandleDragBlock();

        if (Input.GetMouseButtonUp(0) && _selectedblock != null)
            HandleReleaseBlock();
    }
    private void HandleSelectBlock()
    {
        var mousePosition = Input.mousePosition;
        var ray = _mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance, _draggableLayer))
        {
            _selectedblock = hit.collider.gameObject;
            _selectPosition = _selectedblock.transform.position;

            if (Physics.Raycast(ray, out RaycastHit groundHit, _rayDistance, _groundLayer))
            {
                _offset = _selectedblock.transform.position - groundHit.point;
                Debug.Log($"Seçilen Block: {_selectedblock.name} and Týklanan Pozisyon: {_offset}");
            }
        }
    }
    private void HandleDragBlock()
    {
        var mousePosition = Input.mousePosition;
        var ray = _mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance, _groundLayer))
        {
            Vector3 targetPosition = hit.point + _offset;
            _selectedblock.transform.position = targetPosition;
        }

        Debug.Log($"{_selectedblock.name} hareket ettiriliyor.");
    }
    private void HandleReleaseBlock()
    {
        if (!_boardManager.TryPlaceShape(_selectedblock))
        {
            //TODO Go back to position
            Debug.Log("Selected Block can not released!");
            _selectedblock.transform.position = _selectPosition;
            _selectPosition = Vector3.zero;
            _selectedblock = null;
            return;
        }

        Debug.Log($"{_selectedblock.name} yerleþtirildi.");
        _selectedblock.layer = LayerMask.NameToLayer("Placed");
        _selectedblock = null;
    }

}
