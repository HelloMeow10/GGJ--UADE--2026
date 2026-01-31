using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Assassin SelectedAssassin = Assassin.Jorge;
    public static bool IsInDialogueMode = false;

    public static WaitForSeconds ContentSizeFitterFix = new(0.1f);

    public static Action<DataDialogue> OnDialogueStart;
    public static Action OnDialogueEnd;
}