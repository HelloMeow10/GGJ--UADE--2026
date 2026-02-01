using System;
using UnityEngine;
using UnityEngine.UI;

public class ControllerTalkSelection : BaseTabController
{
    [Header("References")]
    [SerializeField] private Button _toDiaryButton;
    [SerializeField] private Button _toAssassinSelectorButton;

    [Header("Talk Buttons")]
    [SerializeField] private Button _talkJuanButton;
    [SerializeField] private Button _talkRobertoButton;

    public static Action OnRequestUnlockNextCharacter;

    protected override void Awake()
    {
        base.Awake();

        OnRequestUnlockNextCharacter += UnlockNextCharacter;
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
        OnRequestUnlockNextCharacter -= UnlockNextCharacter;
        _toDiaryButton.onClick.RemoveAllListeners();
        _toAssassinSelectorButton.onClick.RemoveAllListeners();
    }

    private void UnlockNextCharacter()
    {
        if (!_talkJuanButton.interactable)
        {
            _talkJuanButton.interactable = true;
            return;
        }

        else if (!_talkRobertoButton.interactable)
        {
            _talkRobertoButton.interactable = true;
            
            return;
        }

        else
            _toAssassinSelectorButton.interactable = true;
    }
}
