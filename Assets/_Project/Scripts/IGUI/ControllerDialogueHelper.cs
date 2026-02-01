using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class ControllerDialogueHelper : MonoBehaviour
{
    [Header("music")]
    [SerializeField] private AudioClip music;
    [Header("Settings")]
    [SerializeField] private DataDialogue _dialogueData;
    [SerializeField] private LocalizedString _playerName;
    [SerializeField] private LocalizedString _narratorName;
    [SerializeField, Range(1, 10)] private int _mouthSwapInterval = 5;
    
    [Header("References")]
    [SerializeField] private CanvasGroup _selfCanvasGroup;
    [SerializeField] private Image _suspectPortrait;
    [SerializeField] private Image _eyesReference;
    [SerializeField] private Image _mouthReference;
    
    [Header("Sprites")]
    [SerializeField] private Sprite _mouthCloseSprite;
    [SerializeField] private Sprite _mouthOpenSprite;
    [SerializeField] private Sprite _normalEyes;
    [SerializeField] private Sprite _blinkEyes;

    private int _currentLineIndex = 0;
    private int _maxIndex = 0;
    private bool _canSkip = false;
    private Coroutine _blinkCoroutine = null;
    
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
        if (character != _dialogueData.Character)
        {
            CanvasGroupStatus(false);
            return;
        }

        #if UNITY_EDITOR
        Debug.Log($"[ControllerDialogueHelper] Starting dialogue with character {character}");
        #endif

        _canSkip = true; // Allow skipping from the first line
        GameManager.OnDialogueStart -= OnDialogueStart;
        StartCoroutine(TalkingCoroutine());
        
        // Start blinking animation
        if (_eyesReference != null && _normalEyes != null && _blinkEyes != null)
            _blinkCoroutine = StartCoroutine(BlinkingCoroutine());

        PlayerAnims.OnChangeEyeSprite?.Invoke(true);
        CanvasGroupStatus(true);
        AudioManager.Instance.PlayMusic(music);
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
            yield return StartCoroutine(TypewriterEffect(talkerName, fullText, dialogueLine.Talker));

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
        PlayerAnims.OnChangeEyeSprite?.Invoke(false);

        // Force switch tab back to chat selector & reset index
        ControllerIGUI.OnTabChange?.Invoke(IGUITab.ChatSelector);
        _currentLineIndex = 0;
        GameManager.OnDialogueStart += OnDialogueStart;
        CanvasGroupStatus(false);

        // Stop blinking
        if (_blinkCoroutine != null)
        {
            StopCoroutine(_blinkCoroutine);
            _blinkCoroutine = null;
        }

        StopAllCoroutines();
    }

    private IEnumerator TypewriterEffect(string talkerName, string fullText, TalkerType talker)
    {
        string currentText = "";
        bool insideTag = false;
        int visibleCharacterCount = 0;
        bool isMouthOpen = false;

        if (talker == TalkerType.Narrator)
            fullText = $"<i>{fullText}</i>";

        if (talker == TalkerType.Character && _mouthReference != null)
            _mouthReference.sprite = _mouthCloseSprite;

        else
            PlayerAnims.OnChangeMouthSprite?.Invoke(false);
        
        for (int i = 0; i < fullText.Length; i++)
        {
            char currentChar = fullText[i];
            currentText += currentChar;
            
            // Track when we're inside a rich text tag
            if (currentChar == '<')
                insideTag = true;

            else if (currentChar == '>')
            {
                insideTag = false;

                // Display immediately after closing the tag
                ControllerDialogue.OnDialogueText?.Invoke(talkerName, currentText, talker, _suspectPortrait, _mouthReference, _eyesReference);
                continue;
            }
            
            // Only update display and wait if we're not inside a tag
            if (!insideTag)
            {
                ControllerDialogue.OnDialogueText?.Invoke(talkerName, currentText, talker, _suspectPortrait, _mouthReference, _eyesReference);
                visibleCharacterCount++;
                
                // Animate mouth for character talker
                if (talker == TalkerType.Character && _mouthReference != null)
                {
                    if (visibleCharacterCount % _mouthSwapInterval == 0)
                    {
                        isMouthOpen = !isMouthOpen;
                        _mouthReference.sprite = isMouthOpen ? _mouthOpenSprite : _mouthCloseSprite;
                    }
                }

                // Animate mouth for player talker
                else if (talker == TalkerType.Player)
                {
                    if (visibleCharacterCount % _mouthSwapInterval == 0)
                    {
                        isMouthOpen = !isMouthOpen;
                        PlayerAnims.OnChangeMouthSprite?.Invoke(isMouthOpen);
                    }
                }
                
                // Allow skipping the typewriter effect
                if (Input.anyKeyDown && _canSkip 
                && !GameManager.IsHoveringIGUIButtons 
                && GameManager.CurrentIGUITab == IGUITab.Dialogue)
                {
                    ControllerDialogue.OnDialogueText?.Invoke(talkerName, fullText, talker, _suspectPortrait, _mouthReference, _eyesReference);
                    
                    // Ensure mouth ends closed when skipped
                    if (talker == TalkerType.Character && _mouthReference != null)
                        _mouthReference.sprite = _mouthCloseSprite;

                    else if (talker == TalkerType.Player)
                        PlayerAnims.OnChangeMouthSprite?.Invoke(false);
                    
                    yield break;
                }
                
                yield return GameManager.TypewriterDelay;
            }
        }
        
        // Ensure mouth ends closed
        if (talker == TalkerType.Character && _mouthReference != null)
            _mouthReference.sprite = _mouthCloseSprite;

        else if (talker == TalkerType.Player)
            PlayerAnims.OnChangeMouthSprite?.Invoke(false);
    }

    private IEnumerator BlinkingCoroutine()
    {
        // Initialize with normal eyes
        if (_eyesReference != null)
            _eyesReference.sprite = _normalEyes;
        
        while (true)
        {
            yield return GameManager.BlinkDelay;
            
            // Blink: close eyes
            if (_eyesReference != null && _blinkEyes != null)
                _eyesReference.sprite = _blinkEyes;
            
            // Keep eyes closed for the blink duration
            yield return GameManager.BlinkDelayClose;
            
            // Open eyes
            if (_eyesReference != null && _normalEyes != null)
                _eyesReference.sprite = _normalEyes;
        }
    }

    private void CanvasGroupStatus(bool isEnabled)
    {
        _selfCanvasGroup.alpha = isEnabled ? 1f : 0f;
        _selfCanvasGroup.interactable = isEnabled;
        _selfCanvasGroup.blocksRaycasts = isEnabled;
    }
}
