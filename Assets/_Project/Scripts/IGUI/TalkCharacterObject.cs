using UnityEngine;
using UnityEngine.UI;

public class TalkCharacterObject : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Assassin _character;

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
        GameManager.IsInDialogueMode = true;
        GameManager.OnDialogueStart?.Invoke(_character);
        ControllerIGUI.OnTabChange?.Invoke(IGUITab.Dialogue);
    }
}
