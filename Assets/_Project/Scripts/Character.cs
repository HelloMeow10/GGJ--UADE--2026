using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CanvasGroup))]
public class Character : MonoBehaviour
{
    private Animator animator;
    private CanvasGroup canvasGroup;
    public string talkerName;
    private void Start() {
        animator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Hablar()
    {
        animator.SetTrigger("Talk");
        Debug.Log("hablo");
    }
    public void Oscurecer()
    {
        canvasGroup.alpha = 0f;
    }
    public void Iluminar()
    {
        canvasGroup.alpha = 1f;
    }
}
