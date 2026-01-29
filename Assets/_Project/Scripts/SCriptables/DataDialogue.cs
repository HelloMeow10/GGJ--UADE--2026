using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataDialogue", menuName = "Custom Scriptables/DataDialogue")]
public class DataDialogue : ScriptableObject
{
    public List<DialogueLine> Dialogue = new();

}

[Serializable]
public class DialogueLine
{
    public string text;
    public string talker;
    public Sprite characterSprite;
}
