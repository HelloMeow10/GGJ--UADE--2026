using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AssassinEntry : MonoBehaviour, IAssassinEntry
{
    [Header("Assassin Entry Settings")]
    [SerializeField] private Assassin _characterTarget;

    [Header("Assassin Entry References")]
    [SerializeField] private AssassinNoteEntry _assassinNoteEntryPrefab;
    [SerializeField] private Button _selfButton;
    [SerializeField] private Transform _notesContainer;

    private Coroutine _fixSizeFitterCoroutine;

    public static Action<Assassin, string> OnAssassinEntryAssigned;
    public static Action OnForceRefresh;

    protected void Awake()
    {
        _selfButton.onClick.AddListener(OnAssassinSelected);

        ControllerAssassinSelector.OnAssassinSelected += OnAssassinSelected;
        OnAssassinEntryAssigned += EntryAssigned;
        OnForceRefresh += ForceRefreshCoroutine;
    }

    protected void OnDestroy()
    {
        _selfButton.onClick.RemoveListener(OnAssassinSelected);

        ControllerAssassinSelector.OnAssassinSelected -= OnAssassinSelected;
        OnAssassinEntryAssigned -= EntryAssigned;
        OnForceRefresh -= ForceRefreshCoroutine;
    }

    public void OnAssassinSelected(Assassin character)
    {
        _selfButton.interactable = character != _characterTarget;
    }

    private void OnAssassinSelected()
    {
        ControllerAssassinSelector.OnAssassinSelected?.Invoke(_characterTarget);
        GameManager.SelectedAssassin = _characterTarget;
        AudioManager.Instance.PlayTypewriterSFX(TalkerType.Player);

        #if UNITY_EDITOR
        Debug.Log($"<color=green>Assassin {_characterTarget} selected.</color>");
        #endif
    }

    private void EntryAssigned(Assassin target, string text)
    {
        if (target != _characterTarget || string.IsNullOrEmpty(text)) 
            return;

        AssassinNoteEntry newNoteEntry = Instantiate(_assassinNoteEntryPrefab, _notesContainer);
        newNoteEntry.SetText(text);

        newNoteEntry.transform.SetAsFirstSibling();
    }

    private void ForceRefreshCoroutine()
    {
        if (_fixSizeFitterCoroutine != null)
        {
            StopCoroutine(_fixSizeFitterCoroutine);
            _fixSizeFitterCoroutine = null;
        }

        _fixSizeFitterCoroutine = StartCoroutine(FixContentSizeFitter());
    }

    // Dynamic text elements sometimes don't update their size correctly.
    private IEnumerator FixContentSizeFitter()
    {
        _notesContainer.gameObject.SetActive(false);
        yield return GameManager.ContentSizeFitterFix;
        _notesContainer.gameObject.SetActive(true);
    }
}

public enum Assassin
{
    Jorge,
    Juan,
    Roberto,
}