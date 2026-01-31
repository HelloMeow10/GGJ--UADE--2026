using UnityEngine;
using UnityEngine.UI;

public class TalkCharacterObject : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Assassin _character;
    [SerializeField] private DataDialogue _dialogue;

    [Header("References")]
    [SerializeField] private Button _selfButton;

    protected void Awake()
    {
        _selfButton.onClick.AddListener(OnSelected);
    }

    protected void OnDestroy()
    {
        _selfButton.onClick.RemoveListener(OnSelected);
    }

    private void OnSelected()
    {
        if (_dialogue == null)
        {
            Debug.LogWarning($"[TalkCharacterObject] Dialogue data is null for character {_character}");
            return;
        }

        #if UNITY_EDITOR
        Debug.Log($"[TalkCharacterObject] Selected character {_character} to start dialogue {_dialogue.Character}");
        #endif

        GameManager.IsInDialogueMode = true;
        GameManager.OnDialogueStart?.Invoke(_dialogue);
        ControllerIGUI.OnTabChange?.Invoke(IGUITab.Dialogue);
    }
}
