using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class SimpleMainMenu : MonoBehaviour
{
    public Button startGameButton;
    public Button changeLanguage;
    public TextMeshProUGUI currentLanguageText;

    public string gameSceneName;
    bool _canChangeLanguage = true;
    

    protected void Awake()
    {
        startGameButton.onClick.AddListener(ToGameScene);
        changeLanguage.onClick.AddListener(ChangeLanguageButton);
    }

    protected void OnDestroy()
    {
        startGameButton.onClick.RemoveListener(ToGameScene);
        changeLanguage.onClick.RemoveListener(ChangeLanguageButton);
    }

    void Start()
    {
        StartCoroutine(IntitializeLocalization());
    }

    private void ToGameScene()
    {
        startGameButton.interactable = false;
        changeLanguage.interactable = false;
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameSceneName);
    }

    private void ChangeLanguageButton()
    {
        if (_canChangeLanguage)
        {
            _canChangeLanguage = false;
            StartCoroutine(ChangeLanguage());
        }
    }

    // Prevent spamming language change
    private IEnumerator ChangeLanguage()
    {
        startGameButton.interactable = false;
        changeLanguage.interactable = false;
        var selectedLocale = LocalizationSettings.SelectedLocale;
        var availableLocales = LocalizationSettings.AvailableLocales.Locales;
        int currentIndex = availableLocales.IndexOf(selectedLocale);
        int nextIndex = (currentIndex + 1) % availableLocales.Count;
        LocalizationSettings.SelectedLocale = availableLocales[nextIndex];
        ShortTextLocaleName();

        yield return GameManager.DialogueDelay;
        _canChangeLanguage = true;

        startGameButton.interactable = true;
        changeLanguage.interactable = true;
    }

    private IEnumerator IntitializeLocalization()
    {
        yield return LocalizationSettings.InitializationOperation;

        ShortTextLocaleName();
        startGameButton.interactable = true;
        changeLanguage.interactable = true;
    }

    private void ShortTextLocaleName()
    {
        var selectedLocale = LocalizationSettings.SelectedLocale;
        currentLanguageText.text = selectedLocale.Identifier.CultureInfo.TwoLetterISOLanguageName.ToUpper();
    }
}
