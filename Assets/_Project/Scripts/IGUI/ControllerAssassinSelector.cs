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
        _confirmAssassinButton.onClick.AddListener(OnConfirmAssassinButtonPressed);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        OnAssassinSelected -= OnAssassinFirstSelected;
        _confirmAssassinButton.onClick.RemoveListener(OnConfirmAssassinButtonPressed);
    }

    private void OnAssassinFirstSelected(Assassin character)
    {
        OnAssassinSelected -= OnAssassinFirstSelected;
        _confirmAssassinButton.interactable = true;
    }

    private void OnConfirmAssassinButtonPressed()
    {
        _confirmAssassinButton.interactable = false;
        GameManager.OnAssassinConfirmed?.Invoke();
        
        #if UNITY_EDITOR
        Debug.Log($"<color=green>Assassin {GameManager.SelectedAssassin} confirmed.</color>");
        #endif
    }

    public override void OnExitTab()
    {
        ControllerIGUI.OnTabChange?.Invoke(GameManager.IsInDialogueMode ? IGUITab.Dialogue : IGUITab.ChatSelector);
    }
}
