using UnityEngine;
using UnityEngine.UI;

public class BaseTabController : MonoBehaviour
{
    [Header("BaseTab References")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _exitButton;

    public void HideOrShowTab(bool show)
    {
        _canvasGroup.alpha = show ? 1 : 0;
        _canvasGroup.blocksRaycasts = show;
        _canvasGroup.interactable = show;
    }

    protected virtual void Awake()
    {
        if (_exitButton == null)
            return;
        
        _exitButton.onClick.AddListener(() =>
        {
            ControllerIGUI.OnTabChange?.Invoke(IGUITab.ChatSelector);
        });
    }

    protected virtual void OnDestroy()
    {
        if (_exitButton == null)
            return;
            
        _exitButton.onClick.RemoveAllListeners();
    }
}
