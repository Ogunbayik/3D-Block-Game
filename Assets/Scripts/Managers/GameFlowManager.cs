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
    private UIManager _uiManager;

    public GameFlowManager(SignalBus signalBus, BoardManager boardManager, SpawnManager spawnManager, GameManager gameManager, UIManager uiManager)
    {
        _signalBus = signalBus;
        _boardManager = boardManager;
        _spawnManager = spawnManager;
        _uiManager = uiManager;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<GameSignal.OnGameStateChanged>(OnGamePrepareReceived);
        _signalBus.Subscribe<GameSignal.OnShapePlaced>(OnShapePlacedReceived);
    }
    public void Dispose()
    {
        _signalBus.Unsubscribe<GameSignal.OnGameStateChanged>(OnGamePrepareReceived);
        _signalBus.Unsubscribe<GameSignal.OnShapePlaced>(OnShapePlacedReceived);
    }
    private void OnGamePrepareReceived(GameSignal.OnGameStateChanged signal)
    {
        if (signal.NewState != GameState.Prepare)
            return;

        PrepareLevelAsyn().Forget();
    }
    private void OnShapePlacedReceived(GameSignal.OnShapePlaced signal) => HandleTurnAsyn(signal).Forget();
    public async UniTask PrepareLevelAsyn()
    {
        //Karartma efekti eklenecek.
        Debug.Log($"UI kapatýldý ve Geçiþ animasyonu baþlatýldý.");
        _uiManager.OnGamePrepare();
        await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: _cts.Token);

        //Board oluþturuyoruz. Animasyon ekleyebiliriz.
        _boardManager.GeneratedBoard();
        Debug.Log($"Board oluþturuldu.");

        await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: _cts.Token);

        //Level Setup Edeceðiz. Animasyon ekleyebiliriz.
        _boardManager.SetupLevel();

        await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: _cts.Token);

        Debug.Log($"Level Setup Edildi..");
        _spawnManager.SpawnNewShape();
        //Shapeler oluþturulacak. Animasyon ekleyebiliriz.

        await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: _cts.Token);

        Debug.Log($"Yerleþtirilecek blocklar oluþturuldu..");

        //TODO PlayerController aktif edilecek..
        Debug.Log("PlayerController aktif edildi..");
        _signalBus.Fire(new GameSignal.OnGameStateChanged(GameState.Playing));
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
