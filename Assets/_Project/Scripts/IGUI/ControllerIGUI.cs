using System;
using UnityEngine;

public class ControllerIGUI : MonoBehaviour
{
    public IGUITab CurrentTab = IGUITab.ChatSelector;

    [Header("References")]
    [SerializeField] private BaseTabController _tabTalkSelection;
    [SerializeField] private BaseTabController _tabNotepad;
    [SerializeField] private BaseTabController _tabAssassinSelector;
    
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

        CurrentTab = newTab;
        _tabNotepad.HideOrShowTab(newTab == IGUITab.Notepad);
        _tabAssassinSelector.HideOrShowTab(newTab == IGUITab.AssassinSelector);
        _tabTalkSelection.HideOrShowTab(newTab == IGUITab.ChatSelector);

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