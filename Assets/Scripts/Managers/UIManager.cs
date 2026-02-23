using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIManager : MonoBehaviour
{
    private SignalBus _signalBus;

    [Header("Panel References")]
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _levelSelectPanel;

    [Inject]
    public void Construct(SignalBus signalBus) => _signalBus = signalBus;
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
    private void ToggleMenuPanel(bool isActive) => _menuPanel.SetActive(isActive);
    private void ToggleLevelSelectPanel(bool isActive) => _levelSelectPanel.SetActive(isActive);
}
