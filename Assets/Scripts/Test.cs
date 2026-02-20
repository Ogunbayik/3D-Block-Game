using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public GameManager GameManager;
    public Button StartButton;
    public Button GameOverButton;
    void Start()
    {
        StartButton.onClick.AddListener(StartGame);
        GameOverButton.onClick.AddListener(GameOver);
    }

    public void StartGame()
    {
        GameManager.SwitchState(GameState.Playing);
    }
    public void GameOver()
    {
        GameManager.SwitchState(GameState.GameOver);
    }
}
