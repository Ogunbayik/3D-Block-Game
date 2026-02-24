using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelButton : MonoBehaviour
{
    private SignalBus _signalBus;


    [Header("Level Settings")]
    [SerializeField] private int _levelID;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Button _levelButton;
    //TEST
    [SerializeField] private Image _buttonImage;

    private bool _isLocked;
    public bool IsLocked => _isLocked;

    [Inject]
    public void Construct(SignalBus signalBus) => _signalBus = signalBus;
    private void Awake() => SetLevelText(_levelID);
    private void Start() => _levelButton.onClick.AddListener(() => HandleClickButton());
    private void HandleClickButton()
    {
        if (_isLocked)
        {
            //TODO Level için titreme efekti eklenecek.. (Image olarak kilit iþareti ile ayarlama yapýlabilir)
            Debug.Log($"Level {_levelID} is locked! You can not enter this level..");
        }
        else
        {
            //TODO Level için karartma efekti eklenecek ve Level Datasý ayarlanacak.. GameFlow ile yönetilecek ki asenkron þeklinde sorunsuz çalýþtýralým.
            Debug.Log($"Level {_levelID} is unlocked! Level is opening");
            _signalBus.Fire(new GameSignal.OnClickLevelButton(this));
        }
    }
    private void SetLevelText(int ID) => _levelText.text = $"Level {ID}";
    public void SetLockedStatus(bool isActive) => _isLocked = isActive;
    public int LevelID => _levelID;


    //Test
    public void SetButtonColor(Color color) => _buttonImage.color = color;
}
