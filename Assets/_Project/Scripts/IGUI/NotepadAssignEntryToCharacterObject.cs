using UnityEngine;
using UnityEngine.UI;

public class NotepadAssignEntryToCharacterObject : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Assassin _character;

    [Header("References")]
    [SerializeField] private Button _selfButton;

    protected void Awake()
    {
        _selfButton.onClick.AddListener(OnAssigned);
    }

    protected void OnDestroy()
    {
        _selfButton.onClick.RemoveListener(OnAssigned);
    }

    private void OnAssigned()
    {
        ControllerNotepad.OnNoteAssignedToCharacter?.Invoke(_character);
    }
}
