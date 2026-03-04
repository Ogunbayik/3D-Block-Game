using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIManager : MonoBehaviour
{
    [Header("Panel References")]
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _levelSelectPanel;

    private void Start()
    {
        ToggleMenuPanel(true);
        ToggleLevelSelectPanel(false);
    }
    public void HandleClickedPlayButton()
    {
        ToggleMenuPanel(false);
        ToggleLevelSelectPanel(true);
    }
    public void HandleClickedBackButton()
    {
        ToggleMenuPanel(true);
        ToggleLevelSelectPanel(false);
    }
    public void HandleClickedExitButton()
    {
        //TODO Application quit iþlemi yapýlacak.
        Debug.Log("Clicked Exit Button!");
    }
    public void OnGamePrepare()
    {
        ToggleLevelSelectPanel(false);
        ToggleMenuPanel(false);
    }
    private void ToggleMenuPanel(bool isActive) => _menuPanel.SetActive(isActive);
    private void ToggleLevelSelectPanel(bool isActive) => _levelSelectPanel.SetActive(isActive);
}
