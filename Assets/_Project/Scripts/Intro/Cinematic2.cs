using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Cinematic2 : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text tmpText;
    [SerializeField] private CinematicScene[] scenes;
    [SerializeField] private UnityEvent onEndOfCinematic;

    private int currentSceneIndex = 0;
    private const float fadeDuration = 1f;

    private void Start()
    {
        if (scenes == null || scenes.Length == 0) return;

        image.canvasRenderer.SetAlpha(0f);
        ExecuteScene(scenes[0]);
        image.CrossFadeAlpha(1f, fadeDuration, false);
    }

    // Llamado desde botón
    public void NextScene()
    {
        currentSceneIndex++;

        if (currentSceneIndex < scenes.Length)
        {
            ExecuteScene(scenes[currentSceneIndex]);
        }
        else
        {
            image.CrossFadeAlpha(0f, fadeDuration, false);
            onEndOfCinematic?.Invoke();
        }
    }

    private void ExecuteScene(CinematicScene scene)
    {
        if (scene.sprite != null)
        {
            image.canvasRenderer.SetAlpha(0f);
            image.sprite = scene.sprite;
            image.CrossFadeAlpha(1f, fadeDuration, false);
        }
            

        tmpText.text = scene.text;
    }
}

