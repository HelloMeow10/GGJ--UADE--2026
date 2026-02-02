using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssassinNoteEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Button _deleteButton;

    protected void Awake()
    {
        _deleteButton.onClick.AddListener(OnDeleteEntry);
    }

    protected void OnDestroy()
    {
        _deleteButton.onClick.RemoveListener(OnDeleteEntry);
    }

    private void OnDeleteEntry()
    {
        AudioManager.Instance.PlayTypewriterSFX(TalkerType.Player);
        Destroy(gameObject);
    }

    public void SetText(string text)
    {
        _text.text = text;

        // Resets size fitter.
        _text.gameObject.SetActive(true);
    }
}
