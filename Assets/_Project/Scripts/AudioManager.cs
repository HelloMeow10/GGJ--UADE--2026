using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private List<AudioClipMapping> _backgroundMusics;
    [SerializeField] private List<CharacterMusicMapping> _characterMusics;

    private readonly Dictionary<IGUITab, AudioClipMapping> _tabsMusicDict = new();
    private readonly Dictionary<Assassin, CharacterMusicMapping> _charactersMusicDict = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        foreach (var mapping in _backgroundMusics)
        {
            if (!_tabsMusicDict.ContainsKey(mapping.Tab))
            {
                #if UNITY_EDITOR
                Debug.Log($"[AudioManager] Adding music mapping for tab: {mapping.Tab} | Clip: {mapping.MusicClip.name}");
                #endif
                _tabsMusicDict.Add(mapping.Tab, mapping);
            }
        }

        foreach (var mapping in _characterMusics)
        {
            if (!_charactersMusicDict.ContainsKey(mapping.Character))
            {
                #if UNITY_EDITOR
                Debug.Log($"[AudioManager] Adding music mapping for character: {mapping.Character} | Clip: {mapping.MusicClip.name}");
                #endif

                _charactersMusicDict.Add(mapping.Character, mapping);
            }
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    // ===================== MUSIC =====================

    // Use CurrentIGUITab to determine which music to play
    public void PlayMusic(float volume = 1f)
    {
        AudioClipMapping mapping;
        #if UNITY_EDITOR
        Debug.Log($"[AudioManager] PlayMusic called for tab: {GameManager.CurrentIGUITab} | Character: {GameManager.CurrentTalkingCharacter}");
        #endif

        // If is not dialogue, choose music based on current tab
        if (GameManager.CurrentIGUITab != IGUITab.Dialogue)
        {
            _tabsMusicDict.TryGetValue(GameManager.CurrentIGUITab, out mapping);
            if (mapping == null)
            {
                #if UNITY_EDITOR
                Debug.LogWarning($"[AudioManager] No music mapping found for tab: {GameManager.CurrentIGUITab}");
                #endif
                return;
            }
        }

        // But if it is, then get the track based on the current talking character
        else
        {
            _charactersMusicDict.TryGetValue(GameManager.CurrentTalkingCharacter, out var charMapping);

            if (charMapping == null)
            {
                #if UNITY_EDITOR
                Debug.LogWarning($"[AudioManager] No music mapping found for character: {GameManager.CurrentTalkingCharacter}");
                #endif
                return;
            }

            mapping = charMapping;
        }


        musicSource.Stop();
        musicSource.clip = mapping.MusicClip;
        musicSource.loop = mapping.Loop;
        musicSource.volume = volume;
        musicSource.Play();
    }

    public void PlayMusic(AudioClip clip, bool loop = true, float volume = 1f)
    {
        if (clip == null) return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.volume = volume;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    // ===================== SFX =====================

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        sfxSource.PlayOneShot(clip, volume);
    }

    // ===================== VOLUME =====================

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}

[Serializable]
public class AudioClipMapping
{
    public IGUITab Tab;
    public AudioClip MusicClip;
    public bool Loop = true;
}

[Serializable]
public class CharacterMusicMapping : AudioClipMapping
{
    public Assassin Character;
}