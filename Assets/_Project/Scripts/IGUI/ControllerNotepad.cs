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
    
    [Header("Canvas Groups")]
    [SerializeField] private CanvasGroup _newEntryCanvasGroup;
    [SerializeField] private CanvasGroup _mainActionsCanvasGroup;    

    [Header("Buttons")]
    [SerializeField] private Button _newEntryButton;
    [SerializeField] private Button _saveEntryButton;
    [SerializeField] private Button _cancelEntryButton;

    private INotepadEntry _entryBeingModified;
    public static Action<INotepadEntry> OnModifyNotepadEntryRequest;

    protected void Awake()
    {
        OnModifyNotepadEntryRequest += ModifyEntryRequest;
        _cancelEntryButton.onClick.AddListener(OnCancelNewEntry);

        _newEntryButton.onClick.AddListener(OnNewEntryButtonPressed);
        _saveEntryButton.onClick.AddListener(() =>
        {
            OnSavedNewEntry(_newEntryInputField.text);
        });
    }

    protected void OnDestroy()
    {
        OnModifyNotepadEntryRequest -= ModifyEntryRequest;
        _cancelEntryButton.onClick.RemoveListener(OnCancelNewEntry);

        _newEntryButton.onClick.RemoveListener(OnNewEntryButtonPressed);
        _saveEntryButton.onClick.RemoveAllListeners();
    }

    public void OnNewEntryButtonPressed()
    {
        OnNewEntryStateChange(true);
    }

    private void OnSavedNewEntry(string entryText)
    {
        // Check if the text is valid.
        if (string.IsNullOrEmpty(entryText) || string.IsNullOrWhiteSpace(entryText))
            return;

        OnNewEntryStateChange(false);

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

    private void OnNewEntryStateChange(bool isAddingNewEntry = true)
    {
        _newEntryInputField.text = string.Empty;

        // Swap canvas groups.
        _newEntryCanvasGroup.alpha = isAddingNewEntry ? 1f : 0f;
        _newEntryCanvasGroup.interactable = isAddingNewEntry;
        _newEntryCanvasGroup.blocksRaycasts = isAddingNewEntry;

        _mainActionsCanvasGroup.interactable = !isAddingNewEntry;
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
}
