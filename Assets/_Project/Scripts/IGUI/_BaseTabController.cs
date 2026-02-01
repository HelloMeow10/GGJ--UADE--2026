using UnityEngine;
using UnityEngine.UI;

public class BaseTabController : MonoBehaviour
{
    [Header("music")]
    [SerializeField] private AudioClip music;
    [Header("BaseTab References")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _exitButton;

    public void HideOrShowTab(bool show)
    {
        _canvasGroup.alpha = show ? 1 : 0;
        _canvasGroup.blocksRaycasts = show;
        _canvasGroup.interactable = show;
        if(music != null && show)
        {
            AudioManager.Instance.PlayMusic(music);
        }
    }

    protected virtual void Awake()
    {
        if (_exitButton == null)
            return;


        
        _exitButton.onClick.AddListener(OnExitTab);
    }

    protected virtual void OnDestroy()
    {
        if (_exitButton == null)
            return;
            
        _exitButton.onClick.RemoveListener(OnExitTab);
    }

    public virtual void OnExitTab()
    {
        ControllerIGUI.OnTabChange?.Invoke(IGUITab.ChatSelector);
    }
}
