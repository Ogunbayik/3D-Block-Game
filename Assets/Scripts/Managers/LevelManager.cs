using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private List<LevelDataSO> _levels = new List<LevelDataSO>();

    private LevelDataSO _currentLevel;

    private int _levelIndex = 0;
    private int _maxPassedLevelIndex = 0;
    public LevelDataSO CurrentLevel => _currentLevel;
    private void Start() => _currentLevel = _levels[_levelIndex];
    public void SetLevel(LevelDataSO newLevel) => _currentLevel = newLevel;
    public void IncreaseMaxPassedLevelIndex(LevelDataSO currentLevel) => Mathf.Max(_maxPassedLevelIndex, currentLevel.ID + 1);
}
