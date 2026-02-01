using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Cinematic : MonoBehaviour
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
            image.canvasRenderer.SetAlpha(0f);
            ExecuteScene(scenes[currentSceneIndex]);
            image.CrossFadeAlpha(1f, fadeDuration, false);
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
            image.sprite = scene.sprite;

        tmpText.text = scene.text;

        if (scene.sound != null && AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(scene.sound);
    }
}

[System.Serializable]
public class CinematicScene
{
    public AudioClip sound;
    public string text;
    public Sprite sprite;
}
