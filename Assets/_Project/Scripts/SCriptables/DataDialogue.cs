using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "DataDialogue", menuName = "Custom Scriptables/DataDialogue")]
public class DataDialogue : ScriptableObject
{
    public Assassin Character;
    public string CharacterName;
    public List<DialogueLine> Dialogue = new();
}

[Serializable]
public class DialogueLine
{
    public LocalizedString Line;
    public TalkerType Talker = TalkerType.Narrator;
}

[Serializable]
public enum TalkerType
{
    Character,
    Player,
    Narrator
}