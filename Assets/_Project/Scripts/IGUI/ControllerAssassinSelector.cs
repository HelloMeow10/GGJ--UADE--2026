using System;
using UnityEngine;
using UnityEngine.UI;

public class ControllerAssassinSelector : BaseTabController
{
    [Header("References")]
    [SerializeField] private Button _confirmAssassinButton;
    
    public static Action<Assassin> OnAssassinSelected;

    protected override void Awake()
    {
        base.Awake();
        OnAssassinSelected += OnAssassinFirstSelected;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        OnAssassinSelected -= OnAssassinFirstSelected;
    }

    private void OnAssassinFirstSelected(Assassin character)
    {
        OnAssassinSelected -= OnAssassinFirstSelected;
        _confirmAssassinButton.interactable = true;
    }

    public override void OnExitTab()
    {
        ControllerIGUI.OnTabChange?.Invoke(GameManager.IsInDialogueMode ? IGUITab.Dialogue : IGUITab.ChatSelector);
    }
}
