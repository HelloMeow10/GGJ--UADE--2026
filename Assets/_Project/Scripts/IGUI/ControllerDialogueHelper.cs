using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class ControllerDialogueHelper : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private DataDialogue _dialogueData;
    [SerializeField] private LocalizedString _playerName;
    [SerializeField] private LocalizedString _narratorName;
    
    [Header("References")]
    [SerializeField] private CanvasGroup _selfCanvasGroup;
    [SerializeField] private Image _suspectPortrait;

    private int _currentLineIndex = 0;
    private int _maxIndex = 0;
    private bool _canSkip = false;
    
    private readonly WaitUntil _waitForPlayerInput = new(() => Input.anyKeyDown && !GameManager.IsHoveringIGUIButtons && GameManager.CurrentIGUITab == IGUITab.Dialogue);

    protected void Awake()
    {
        GameManager.OnDialogueStart += OnDialogueStart;
    }

    protected void Start()
    {
        _maxIndex = _dialogueData.Dialogue.Count - 1;
    }

    protected void OnDestroy()
    {
        GameManager.OnDialogueStart -= OnDialogueStart;
        StopAllCoroutines();
    }

    private void OnDialogueStart(Assassin character)
    {
        #if UNITY_EDITOR
        Debug.Log($"[ControllerDialogueHelper] Starting dialogue with character {character}");
        #endif

        _canSkip = true; // Allow skipping from the first line
        StartCoroutine(TalkingCoroutine());
    }

    private string GetTalkerName(TalkerType talker)
    {
        return talker switch
        {
            TalkerType.Character => _dialogueData.CharacterName,
            TalkerType.Player => _playerName.IsEmpty ? "Player" : _playerName.GetLocalizedString(),
            TalkerType.Narrator => _narratorName.IsEmpty ? "Narrator" : _narratorName.GetLocalizedString(),
            _ => "???"
        };
    }

    private IEnumerator TalkingCoroutine()
    {
        while (_currentLineIndex <= _maxIndex)
        {
            DialogueLine dialogueLine = _dialogueData.Dialogue[_currentLineIndex];
            string talkerName = GetTalkerName(dialogueLine.Talker);
            
            // Load localized string asynchronously for WebGL compatibility
            var stringOperation = dialogueLine.Line.GetLocalizedStringAsync();
            yield return stringOperation;
            string fullText = stringOperation.Result;

            // Display text progressively with typewriter effect
            yield return StartCoroutine(TypewriterEffect(talkerName, fullText, dialogueLine.Talker == TalkerType.Narrator));

            // Small delay to prevent accidental skipping
            _canSkip = false;
            yield return GameManager.DialogueDelay;

            // Wait for player input before proceeding to the next line
            yield return _waitForPlayerInput;

            _currentLineIndex++;
            
            // Wait a frame to clear the input buffer before starting the next line
            yield return null;
            _canSkip = true;
        }

        // Dialogue ended
        GameManager.IsInDialogueMode = false;
        GameManager.OnDialogueEnd?.Invoke();
        ControllerTalkSelection.OnRequestUnlockNextCharacter?.Invoke();

        // Force switch tab back to chat selector & reset index
        ControllerIGUI.OnTabChange?.Invoke(IGUITab.ChatSelector);
        _currentLineIndex = 0;
    }

    private IEnumerator TypewriterEffect(string talkerName, string fullText, bool isNarrator = false)
    {
        string currentText = "";
        bool insideTag = false;

        if (isNarrator)
            fullText = $"<i>{fullText}</i>";
        
        for (int i = 0; i < fullText.Length; i++)
        {
            char currentChar = fullText[i];
            currentText += currentChar;
            
            // Track when we're inside a rich text tag
            if (currentChar == '<')
            {
                insideTag = true;
            }
            else if (currentChar == '>')
            {
                insideTag = false;
                // Display immediately after closing the tag
                ControllerDialogue.OnDialogueText?.Invoke(talkerName, currentText);
                continue;
            }
            
            // Only update display and wait if we're not inside a tag
            if (!insideTag)
            {
                ControllerDialogue.OnDialogueText?.Invoke(talkerName, currentText);
                
                // Allow skipping the typewriter effect
                if (Input.anyKeyDown && _canSkip 
                && !GameManager.IsHoveringIGUIButtons 
                && GameManager.CurrentIGUITab == IGUITab.Dialogue)
                {
                    ControllerDialogue.OnDialogueText?.Invoke(talkerName, fullText);
                    yield break;
                }
                
                yield return GameManager.TypewriterDelay;
            }
        }
    }
}
