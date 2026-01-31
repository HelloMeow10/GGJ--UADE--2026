using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControllerDialogue : BaseTabController
{
    [Header("Buttons")]
    [SerializeField] private Button _notepadButton;
    [SerializeField] private Button _assassinSelectorButton;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI _talkerNameText;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private Image _playerPortrait;

    [Header("Chat Canvas Groups")]
    [SerializeField] private CanvasGroup _chatJorge;

    [Header("Others")]
    public Color nonTalkerColor = new(1f, 1f, 1f, 0.5f);
    public Color talkerColor = Color.white;

    public static Action<string, string> OnDialogueText;

    protected override void Awake()
    {
        base.Awake();
        OnDialogueText += SetDialogueText;

        _notepadButton.onClick.AddListener(OpenNotepad);
        _assassinSelectorButton.onClick.AddListener(OpenAssassinSelector);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        OnDialogueText -= SetDialogueText;

        _notepadButton.onClick.RemoveListener(OpenNotepad);
        _assassinSelectorButton.onClick.RemoveListener(OpenAssassinSelector);
    }

    private void OpenNotepad()
    {
        ControllerIGUI.OnTabChange?.Invoke(IGUITab.Notepad);
    }

    private void OpenAssassinSelector()
    {
        ControllerIGUI.OnTabChange?.Invoke(IGUITab.AssassinSelector);
    }

    private void SetDialogueText(string talkerName, string dialogueLine)
    {
        _talkerNameText.text = talkerName;
        _dialogueText.text = dialogueLine;
    }

    public override void OnExitTab()
    {
        ControllerIGUI.OnTabChange?.Invoke(GameManager.IsInDialogueMode ? IGUITab.Dialogue : IGUITab.ChatSelector);
    }
}
