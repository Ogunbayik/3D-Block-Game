using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    private SignalBus _signalBus;

    public GameState CurrentState;

    [Inject]
    public void Construct(SignalBus signalBus) => _signalBus = signalBus;
    public void SwitchState(GameState newState)
    {
        if (newState == CurrentState)
            return;

        CurrentState = newState;

        _signalBus.Fire(new GameSignal.OnGameStateChanged(CurrentState));
    }
}
public enum GameState
{
    MainMenu,
    Playing,
    GameOver,
    GamePass
}
