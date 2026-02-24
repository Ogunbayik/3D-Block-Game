using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class LevelManager : MonoBehaviour
{
    private SignalBus _signalBus;

    [Header("Level Settings")]
    [SerializeField] private List<LevelDataSO> _levels = new List<LevelDataSO>();

    private LevelDataSO _currentLevel;

    private int _levelIndex = 0;
    private int _maxPassedLevelIndex = 0;
    public LevelDataSO CurrentLevel => _currentLevel;

    [Inject]
    public void Construct(SignalBus signalBus) => _signalBus = signalBus;
    private void Awake() => _currentLevel = _levels[_levelIndex];
    public void OnLevelChanged(GameSignal.OnClickLevelButton signal)
    {
        if (signal.LevelButton.IsLocked)
            return;

        var targetIndex = signal.LevelButton.LevelID - 1;
        _levelIndex = targetIndex;
        _signalBus.Fire(new GameSignal.OnGameStateChanged(GameState.Prepare));
    }
    public void IncreaseMaxPassedLevelIndex(LevelDataSO currentLevel) => Mathf.Max(_maxPassedLevelIndex, currentLevel.ID + 1);
}
