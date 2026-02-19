using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ScoreManager : MonoBehaviour
{
    private SignalBus _signalBus;


    [Header("Score Settings")]
    [SerializeField] private int _passScore;
    [SerializeField] private int _blockScore;

    private int _currentScore = 0;

    public int CurrentScore => _currentScore;

    [Inject]
    public void Construct(SignalBus signalBus) => _signalBus = signalBus;
    private void OnEnable() => _signalBus.Subscribe<GameSignal.OnMatchesFound>(HandleAddScore);
    private void OnDisable() => _signalBus.Unsubscribe<GameSignal.OnMatchesFound>(HandleAddScore);
    private void HandleAddScore(GameSignal.OnMatchesFound signal)
    {
        var score = CalculateTotalScore(signal.RowMatchCount, signal.ColMatchCount);
        IncreaseScore(score);
    }

    private int CalculateTotalScore(int rowMatchCount, int colMatchCount)
    {
        var totalRowScore = CalculateLineScore(rowMatchCount);
        var totalColScore = CalculateLineScore(colMatchCount);
        var totalScore = (totalRowScore + totalColScore) / 2;

        return totalScore;
    }
    private int CalculateLineScore(int matchCount)
    {
        var blockCount = 8 * matchCount;

        if (matchCount < 2)
            return blockCount * matchCount;

        var newBlockScore = _blockScore + matchCount;

        return blockCount * newBlockScore;
    }

    private void IncreaseScore(int score) => _currentScore += score;
    private bool IsPassedLevel() => _currentScore >= _passScore;
}
