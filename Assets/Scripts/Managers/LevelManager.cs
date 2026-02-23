using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private List<LevelDataSO> _levels = new List<LevelDataSO>();

    private LevelDataSO _currentLevel;

    private int _levelIndex = 0;
    private int _maxPassedLevelIndex = 0;
    public LevelDataSO CurrentLevel => _currentLevel;
    private void Awake() => _currentLevel = _levels[_levelIndex];
    public void OnLevelChanged(GameSignal.OnClickLevelButton signal)
    {
        if (signal.LevelButton.LevelID == _levelIndex || signal.LevelButton.IsLocked)
            return;

        _levelIndex = signal.LevelButton.LevelID;
    }
    public void IncreaseMaxPassedLevelIndex(LevelDataSO currentLevel) => Mathf.Max(_maxPassedLevelIndex, currentLevel.ID + 1);
}
