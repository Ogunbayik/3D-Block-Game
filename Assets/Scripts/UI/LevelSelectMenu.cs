using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelSelectMenu : MonoBehaviour
{
    private SignalBus _signalBus;

    private LevelManager _levelManager;

    [Header("Button References")]
    [SerializeField] private List<LevelButton> _levelButtons = new List<LevelButton>();
    [SerializeField] private Button _backButton;


    [Inject]
    public void Construct(SignalBus signalBus, LevelManager levelManager)
    { 
        _signalBus = signalBus;
        _levelManager = levelManager;
    }

    void Start()
    {
        _backButton.onClick.AddListener(() => _signalBus.Fire(new GameSignal.OnClickBackButton()));


        var currentLevel = _levelManager.CurrentLevel;
        foreach (var button in _levelButtons)
        {
            if (button == null)
                Debug.Log("Buton bulunamadý.");

            if (currentLevel == null)
                Debug.Log("Level bulunamadý.");

            if(currentLevel.ID < button.LevelID)
            {
                button.SetLockedStatus(true);
                button.SetButtonColor(Color.red);
            }
            else
            {
                button.SetLockedStatus(false);
                button.SetButtonColor(Color.green);
            }
        }
    }
}
