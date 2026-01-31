using System;
using UnityEngine;

public class ControllerIGUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BaseTabController _tabTalkSelection;
    [SerializeField] private BaseTabController _tabNotepad;
    [SerializeField] private BaseTabController _tabAssassinSelector;
    [SerializeField] private BaseTabController _tabDialogue;

    public static Action<IGUITab> OnTabChange;

    protected void Awake()
    {
        OnTabChange += TabChange;
    }

    protected void OnDestroy()
    {
        OnTabChange -= TabChange;
    }

    private void TabChange(IGUITab newTab)
    {
        #if UNITY_EDITOR
        Debug.Log("<color=green>ControllerIGUI</color>: Changing tab to " + newTab);
        #endif

        _tabNotepad.HideOrShowTab(newTab == IGUITab.Notepad);
        _tabAssassinSelector.HideOrShowTab(newTab == IGUITab.AssassinSelector);
        _tabTalkSelection.HideOrShowTab(newTab == IGUITab.ChatSelector);
        _tabDialogue.HideOrShowTab(newTab == IGUITab.Dialogue);

        GameManager.CurrentIGUITab = newTab;

        // Just in case, always force refresh size fitter.
        AssassinEntry.OnForceRefresh?.Invoke();
    }
}

public enum IGUITab
{
    Notepad,
    AssassinSelector,
    ChatSelector,
    Dialogue,
}