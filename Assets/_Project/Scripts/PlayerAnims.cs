using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAnims : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image _eyesReference;
    [SerializeField] private Image _mouthReference;

    [Header("Sprites")]
    [SerializeField] private Sprite _mouthCloseSprite;
    [SerializeField] private Sprite _mouthOpenSprite;
    [SerializeField] private Sprite _normalEyes;
    [SerializeField] private Sprite _blinkEyes;

    public static Action<bool> OnChangeMouthSprite;
    public static Action<bool> OnChangeEyeSprite;

    protected void Awake()
    {
        OnChangeMouthSprite += MouthAnim;
        OnChangeEyeSprite += EyeAnim;
    }

    protected void OnDestroy()
    {
        OnChangeMouthSprite -= MouthAnim;
        OnChangeEyeSprite -= EyeAnim;
    }

    private void MouthAnim(bool isTalking)
    {
        _mouthReference.sprite = isTalking ? _mouthOpenSprite : _mouthCloseSprite;
    }

    private void EyeAnim(bool start)
    {
        if (start)
            StartCoroutine(BlinkingCoroutine());
        else
            StopAllCoroutines();
    }
    
    private IEnumerator BlinkingCoroutine()
    {
        // Initialize with normal eyes
        if (_eyesReference != null)
            _eyesReference.sprite = _normalEyes;
        
        while (true)
        {
            yield return GameManager.BlinkDelayPlayer;
            
            // Blink: close eyes
            if (_eyesReference != null && _blinkEyes != null)
                _eyesReference.sprite = _blinkEyes;
            
            // Keep eyes closed for the blink duration
            yield return GameManager.BlinkDelayPlayerClose;
            
            // Open eyes
            if (_eyesReference != null && _normalEyes != null)
                _eyesReference.sprite = _normalEyes;
        }
    }
}
