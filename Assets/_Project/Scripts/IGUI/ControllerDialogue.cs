using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControllerDialogue : BaseTabController
{
    [Header("Scriptables")]
    [SerializeField] private DataDialogue _dataDialogueJorge;
    [SerializeField] private DataDialogue _dataDialogueJuan;
    [SerializeField] private DataDialogue _dataDialogueRoberto;

    [Header("Buttons")]
    [SerializeField] private Button _notepadButton;
    [SerializeField] private Button _assassinSelectorButton;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private Image _playerPortrait;

    [Header("Chat Canvas Groups")]
    [SerializeField] private CanvasGroup _chatJorge;

    [Header("Others")]
    public Color nonTalkerColor = new(1f, 1f, 1f, 0.5f);
    public Color talkerColor = Color.white;

    protected override void Awake()
    {
        base.Awake();

        _notepadButton.onClick.AddListener(OpenNotepad);
        _assassinSelectorButton.onClick.AddListener(OpenAssassinSelector);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

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

    public override void OnExitTab()
    {
        ControllerIGUI.OnTabChange?.Invoke(GameManager.IsInDialogueMode ? IGUITab.Dialogue : IGUITab.ChatSelector);
    }
}
