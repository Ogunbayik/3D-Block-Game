using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using Zenject;


public class GameFlowManager : IInitializable, IDisposable
{
    private readonly CancellationTokenSource _cts = new CancellationTokenSource();

    private SignalBus _signalBus;

    private BoardManager _boardManager;
    private SpawnManager _spawnManager;

    public GameFlowManager(SignalBus signalBus, BoardManager boardManager, SpawnManager spawnManager, GameManager gameManager)
    {
        _signalBus = signalBus;
        _boardManager = boardManager;
        _spawnManager = spawnManager;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<GameSignal.OnGameStateChanged>(OnGameStartedReceived);
        _signalBus.Subscribe<GameSignal.OnShapePlaced>(OnShapePlacedReceived);
    }
    public void Dispose()
    {
        _signalBus.Unsubscribe<GameSignal.OnGameStateChanged>(OnGameStartedReceived);
        _signalBus.Unsubscribe<GameSignal.OnShapePlaced>(OnShapePlacedReceived);
    }
    private void OnGameStartedReceived(GameSignal.OnGameStateChanged signal)
    {
        if (signal.NewState == GameState.Playing)
            PrepareLevelAsyn().Forget();
    }
    private void OnShapePlacedReceived(GameSignal.OnShapePlaced signal) => HandleTurnAsyn(signal).Forget();
    public async UniTask PrepareLevelAsyn()
    {
        //Board oluþturuyoruz. Animasyon ekleyebiliriz.
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: _cts.Token);
        _boardManager.GeneratedBoard();
        //Level Setup Edeceðiz. Animasyon ekleyebiliriz.
        _boardManager.SetupLevel();
        //Shapeler oluþturulacak. Animasyon ekleyebiliriz.
        _spawnManager.SpawnNewShape();
    }
    public async UniTask HandleTurnAsyn(GameSignal.OnShapePlaced signal)
    {
        //SpawnManagerden objeyi sildik.
        _spawnManager.RemoveActiveShape(signal.Shape);

        _boardManager.ProcessMatches();

        //Eðer eþleþme varsa animasyon kadar bekleyecek
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: _cts.Token);

        if (_spawnManager.ActiveShapes.Count == 0)
            _spawnManager.SpawnNewShape();

        _boardManager.CheckGameOver();
    }

}
