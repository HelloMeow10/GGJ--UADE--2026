using UnityEngine;
using UnityEngine.UI;

public class ControllerTalkSelection : BaseTabController
{
    [Header("References")]
    [SerializeField] private Button _toDiaryButton;
    [SerializeField] private Button _toAssassinSelectorButton;

    protected override void Awake()
    {
        base.Awake();

        _toDiaryButton.onClick.AddListener(() =>
        {
            ControllerIGUI.OnTabChange?.Invoke(IGUITab.Notepad);
        });

        _toAssassinSelectorButton.onClick.AddListener(() =>
        {
            ControllerIGUI.OnTabChange?.Invoke(IGUITab.AssassinSelector);
        });
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _toDiaryButton.onClick.RemoveAllListeners();
        _toAssassinSelectorButton.onClick.RemoveAllListeners();
    }
}
