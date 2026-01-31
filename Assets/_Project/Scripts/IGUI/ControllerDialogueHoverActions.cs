using UnityEngine;
using UnityEngine.EventSystems;

public class ControllerDialogueHoverActions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.IsHoveringIGUIButtons = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.IsHoveringIGUIButtons = false;
    }
}
