using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotepadEntry : MonoBehaviour, INotepadEntry
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI _entryText;
    [SerializeField] private Button _assignButton;
    [SerializeField] private Button _modifyButton;
    [SerializeField] private Button _deleteButton;

    public Transform SelfTransform => transform;

    protected void Awake()
    {
        _assignButton.onClick.AddListener(OnAssignButtonPressed);
        _modifyButton.onClick.AddListener(OnModifyButtonPressed);
        _deleteButton.onClick.AddListener(OnDeleteButtonPressed);   
    }

    protected void OnDestroy()
    {
        _modifyButton.onClick.RemoveListener(OnModifyButtonPressed);
        _deleteButton.onClick.RemoveListener(OnDeleteButtonPressed);
    }

    public string GetText()
    {
        return _entryText.text;
    }

    public void OnDeleteButtonPressed()
    {
        _modifyButton.interactable = false;
        _deleteButton.interactable = false;

        Destroy(gameObject);
    }

    public void OnModifyButtonPressed()
    {
        ControllerNotepad.OnModifyNotepadEntryRequest?.Invoke(this);
    }

    public void OnAssignButtonPressed()
    {
        ControllerNotepad.OnAssignNotepadEntryToAssassinRequest?.Invoke(this);
    }

    public void SetText(string text)
    {
        _entryText.text = text;
    }
}
