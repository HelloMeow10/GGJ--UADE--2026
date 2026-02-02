using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControllerNotepad : BaseTabController
{
    [Header("References")]
    [SerializeField] private NotepadEntry _notepadEntryPrefab;
    [SerializeField] private TMP_InputField _newEntryInputField;
    [SerializeField] private GameObject _entriesParent;
    [SerializeField] private TextMeshProUGUI _assignNoteToAssassinText;
    
    [Header("Canvas Groups")]
    [SerializeField] private CanvasGroup _newEntryCanvasGroup;
    [SerializeField] private CanvasGroup _mainActionsCanvasGroup;
    [SerializeField] private CanvasGroup _assignNoteToAssassinCanvasGroup;

    [Header("Buttons")]
    [SerializeField] private Button _newEntryButton;
    [SerializeField] private Button _saveEntryButton;
    [SerializeField] private Button _cancelEntryButton;
    [SerializeField] private Button _cancelAssignNoteButton;

    private INotepadEntry _entryBeingModified;
    private static string _entryBeingAssigned = string.Empty;

    public static Action<INotepadEntry> OnModifyNotepadEntryRequest;
    public static Action<INotepadEntry> OnAssignNotepadEntryToAssassinRequest;
    public static Action<Assassin> OnNoteAssignedToCharacter;

#region Unity Methods
    protected override void Awake()
    {
        base.Awake();

        OnModifyNotepadEntryRequest += ModifyEntryRequest;
        OnAssignNotepadEntryToAssassinRequest += AssignEntryToAssassinRequest;
        OnNoteAssignedToCharacter += NoteAssignedToCharacter;

        _cancelEntryButton.onClick.AddListener(OnCancelNewEntry);
        _newEntryButton.onClick.AddListener(OnNewEntryButtonPressed);
        _saveEntryButton.onClick.AddListener(() =>
        {
            OnSavedNewEntry(_newEntryInputField.text);
        });

        _cancelAssignNoteButton.onClick.AddListener(OnCancelAssignNote);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        OnModifyNotepadEntryRequest -= ModifyEntryRequest;
        OnAssignNotepadEntryToAssassinRequest -= AssignEntryToAssassinRequest;
        OnNoteAssignedToCharacter -= NoteAssignedToCharacter;

        _cancelEntryButton.onClick.RemoveListener(OnCancelNewEntry);
        _newEntryButton.onClick.RemoveListener(OnNewEntryButtonPressed);
        _saveEntryButton.onClick.RemoveAllListeners();
        _cancelAssignNoteButton.onClick.RemoveListener(OnCancelAssignNote);
    }
#endregion

    public void OnNewEntryButtonPressed()
    {
        AudioManager.Instance.PlayTypewriterSFX(TalkerType.Player);
        OnNewEntryStateChange(true);
    }

    public void OnTextChanged(string newText)
    {
        AudioManager.Instance.PlayTypewriterSFX(TalkerType.Player);
    }

    private void OnSavedNewEntry(string entryText)
    {
        // Check if the text is valid.
        if (string.IsNullOrEmpty(entryText) || string.IsNullOrWhiteSpace(entryText))
            return;

        OnNewEntryStateChange(false);
        AudioManager.Instance.PlayTypewriterSFX(TalkerType.Player);

        // Generate a new entry.
        if (_entryBeingModified == null)
        {
            #if UNITY_EDITOR
            Debug.Log("ControllerNotepad: Creating new notepad entry w/ text: " + entryText);
            #endif

            INotepadEntry newEntry = Instantiate(_notepadEntryPrefab, _entriesParent.transform);
            newEntry.SetText(entryText);

            // Set as first child to appear on top.
            newEntry.SelfTransform.SetAsFirstSibling();

            // Clear the input field.
            _newEntryInputField.text = string.Empty;
        }

        else
        {
            #if UNITY_EDITOR
            Debug.Log("ControllerNotepad: Modifying notepad entry to have text: " + entryText);
            #endif

            _entryBeingModified.SetText(entryText);
        }

        _entryBeingModified = null;
    }

    private void ModifyEntryRequest(INotepadEntry entry)
    {
        if (entry == null)
        {
            #if UNITY_EDITOR
            Debug.LogError("ControllerNotepad: ModifyEntryRequest received a null entry.");
            #endif
            return;
        }

        #if UNITY_EDITOR
        Debug.Log("ControllerNotepad: Modifying notepad entry with current text: " + entry.GetText());
        #endif

        OnNewEntryStateChange(true);
        _entryBeingModified = entry;
        _newEntryInputField.text = entry.GetText();
    }

    private void OnCancelNewEntry()
    {
        _entryBeingModified = null;
        OnNewEntryStateChange(false);
    }

    private void AssignEntryToAssassinRequest(INotepadEntry entry)
    {
        if (entry == null)
        {
            #if UNITY_EDITOR
            Debug.LogError("ControllerNotepad: AssignEntryToAssassinRequest received a null entry.");
            #endif
            return;
        }

        #if UNITY_EDITOR
        Debug.Log("ControllerNotepad: Assigning notepad entry with text: " + entry.GetText() + " to an assassin.");
        #endif

        string entryText = entry.GetText();
        _assignNoteToAssassinText.text = entryText;
        _entryBeingAssigned = entryText;

        OnAssignNoteStateChange(true);
        AudioManager.Instance.PlayTypewriterSFX(TalkerType.Player);
    }

    private void NoteAssignedToCharacter(Assassin character)
    {
        #if UNITY_EDITOR
        Debug.Log($"ControllerNotepad: Notepad entry assigned to character {character}.");
        #endif

        AssassinEntry.OnAssassinEntryAssigned?.Invoke(character, _entryBeingAssigned);
        _entryBeingAssigned = string.Empty;
        OnAssignNoteStateChange(false);
    }

    private void OnCancelAssignNote()
    {
        _entryBeingAssigned = string.Empty;
        OnAssignNoteStateChange(false);
        AudioManager.Instance.PlayTypewriterSFX(TalkerType.Player);
    }

    private void OnNewEntryStateChange(bool isAddingNewEntry = true)
    {
        _newEntryInputField.text = string.Empty;

        // Swap canvas groups.
        _newEntryCanvasGroup.alpha = isAddingNewEntry ? 1f : 0f;
        _newEntryCanvasGroup.interactable = isAddingNewEntry;
        _newEntryCanvasGroup.blocksRaycasts = isAddingNewEntry;

        _mainActionsCanvasGroup.interactable = !isAddingNewEntry;
    }

    private void OnAssignNoteStateChange(bool isActive)
    {
        _assignNoteToAssassinCanvasGroup.alpha = isActive ? 1f : 0f;
        _assignNoteToAssassinCanvasGroup.interactable = isActive;
        _assignNoteToAssassinCanvasGroup.blocksRaycasts = isActive;

        _mainActionsCanvasGroup.interactable = !isActive;

        if (!isActive)
            _assignNoteToAssassinText.text = string.Empty;
    }

    public override void OnExitTab()
    {
        ControllerIGUI.OnTabChange?.Invoke(GameManager.IsInDialogueMode ? IGUITab.Dialogue : IGUITab.ChatSelector);
    }
}
