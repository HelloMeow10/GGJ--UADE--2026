using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static IGUITab CurrentIGUITab = IGUITab.ChatSelector;
    public static Assassin CurrentTalkingCharacter = Assassin.Jorge;

    public static Assassin SelectedAssassin = Assassin.Jorge;
    public static bool IsInDialogueMode = false;
    public static bool IsHoveringIGUIButtons = false;

    public static WaitForSeconds ContentSizeFitterFix = new(0.1f);
    public static WaitForSeconds DialogueDelay = new(0.25f);
    public static WaitForSeconds TypewriterDelay = new(0.015f);

    public static WaitForSeconds BlinkDelay = new(1f);
    public static WaitForSeconds BlinkDelayClose = new(.2f);

    public static WaitForSeconds BlinkDelayPlayer = new(1.25f);
    public static WaitForSeconds BlinkDelayPlayerClose = new(.2f);


    public static Action<Assassin> OnDialogueStart;
    public static Action OnDialogueEnd;
    public static Action OnAssassinConfirmed;
}