using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameMenu : MonoBehaviour
{
    private SignalBus _signalBus;

    [Header("Button References")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _exitButton;

    [Inject]
    public void Construct(SignalBus signalBus) => _signalBus = signalBus;
    private void Start()
    {
        _playButton.onClick.AddListener(() => _signalBus.Fire(new GameSignal.OnClickPlayButton()));
        _exitButton.onClick.AddListener(() => _signalBus.Fire(new GameSignal.OnClickExitButton()));
    }
}
