using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    private BoardManager _boardManager;

    private SignalBus _signalBus;

    [Header("Raycast Settings")]
    [SerializeField] private float _rayDistance;
    [SerializeField] private float _offsetY;
    [Header("Layer Settings")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _draggableLayer;

    private GameObject _selectedShape;

    private Vector3 _offset;
    private Vector3 _selectPosition;

    private Camera _mainCamera;

    [Inject]
    public void Construct(BoardManager boardManager, SignalBus signalBus)
    {
        _boardManager = boardManager;
        _signalBus = signalBus;
    }
    private void Start() => Initialize();
    private void Initialize() => _mainCamera = Camera.main;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            HandleSelectBlock();

        if (Input.GetMouseButton(0) && _selectedShape != null)
            HandleDragBlock();

        if (Input.GetMouseButtonUp(0) && _selectedShape != null)
            HandleReleaseBlock();
    }
    private void HandleSelectBlock()
    {
        var mousePosition = Input.mousePosition;
        var ray = _mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance, _draggableLayer))
        {
            _selectedShape = hit.collider.gameObject;
            _selectPosition = _selectedShape.transform.position;

            if (Physics.Raycast(ray, out RaycastHit groundHit, _rayDistance, _groundLayer))
            {
                _offset = _selectedShape.transform.position - groundHit.point;
                Debug.Log($"Seçilen Block: {_selectedShape.name} and Týklanan Pozisyon: {_offset}");
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
            _selectedShape.transform.position = targetPosition;
        }

        Debug.Log($"{_selectedShape.name} hareket ettiriliyor.");
    }
    private void HandleReleaseBlock()
    {
        if (!_boardManager.TryPlaceShape(_selectedShape))
        {
            //TODO Go back to position
            Debug.Log("Selected Block can not released!");
            _selectedShape.transform.position = _selectPosition;
            _selectPosition = Vector3.zero;
            _selectedShape = null;
            return;
        }

        _signalBus.Fire(new GameSignal.OnShapePlaced(_selectedShape));

        Debug.Log($"{_selectedShape.name} yerleþtirildi.");
        _selectedShape.layer = LayerMask.NameToLayer("Placed");
        _selectedShape = null;
    }

}
