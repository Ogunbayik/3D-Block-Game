using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SpawnSlot : MonoBehaviour
{
    private SignalBus _signalBus;

    private BaseShape _currentShape;
    public BaseShape CurrentShape => _currentShape;

    [Inject]
    public void Construct(SignalBus signalBus) => _signalBus = signalBus;
    private void OnEnable() => _signalBus.Subscribe<GameSignal.OnShapePlaced>(HandleShapePlaced);
    private void OnDisable() => _signalBus.Unsubscribe<GameSignal.OnShapePlaced>(HandleShapePlaced);
    private void HandleShapePlaced(GameSignal.OnShapePlaced signal)
    {
        if (_currentShape == signal.Shape)
            Clear();

        _signalBus.Fire(new GameSignal.OnSlotCleared());
    }
    public bool IsPlaced => _currentShape == null;
    public void AssingnShape(BaseShape shape) => _currentShape = shape;
    public void Clear() => _currentShape = null;
}
