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
    [Header("Layer Settings")]
    [SerializeField] private LayerMask _draggableLayer;
    [SerializeField] private LayerMask _groundLayer;

    private BaseShape _selectedShape;

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
            HandleSelectShape();

        if (Input.GetMouseButton(0) && _selectedShape != null)
            HandleDragShape();

        if (Input.GetMouseButtonUp(0) && _selectedShape != null)
            HandleReleaseShape();
    }
    private void HandleSelectShape()
    {
        var mousePosition = Input.mousePosition;
        var ray = _mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance,_draggableLayer))
        {
            BaseShape touchedShape = hit.collider.GetComponentInParent<BaseShape>();

            if (touchedShape != null)
            {
                _selectedShape = touchedShape;
                _selectPosition = _selectedShape.transform.position;
                _selectedShape.Click();

                if (Physics.Raycast(ray, out RaycastHit groundHit, _rayDistance, _groundLayer))
                {
                    _offset = _selectedShape.transform.position - groundHit.point;
                }
            }
            else
                _selectedShape = null;
        }
    }
    private void HandleDragShape()
    {
        var mousePosition = Input.mousePosition;
        var ray = _mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance, _groundLayer))
        {
            Vector3 targetPosition = hit.point + _offset;
            _selectedShape.transform.position = targetPosition;
        }
    }
    private void HandleReleaseShape()
    {
        if (!_boardManager.TryPlaceShape(_selectedShape))
        {
            //TODO Go back to position with animation
            _selectedShape.Released();
            _selectedShape = null;
            return;
        }

        _signalBus.Fire(new GameSignal.OnShapePlaced(_selectedShape));

        _selectedShape = null;
    }
}
