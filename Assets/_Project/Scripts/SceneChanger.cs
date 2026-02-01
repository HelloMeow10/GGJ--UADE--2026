using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string winScene;
    public string loseScene;

    protected void Awake()
    {
        GameManager.OnAssassinConfirmed += OnAssassinConfirmed;
    }

    protected void OnDestroy()
    {
        GameManager.OnAssassinConfirmed -= OnAssassinConfirmed;
    }

    private void OnAssassinConfirmed()
    {
        switch (GameManager.SelectedAssassin)
        {
            case Assassin.Jorge:
                LoadScene(winScene);
                break;
            case Assassin.Juan:
            case Assassin.Roberto:
                LoadScene(loseScene);
                break;
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
