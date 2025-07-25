using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Boutons")]
    public Button playButton;
    public Button creditsButton;
    public Button quitButton;

    void Start()
    {
        playButton.onClick.AddListener(LoadGameScene);
        creditsButton.onClick.AddListener(LoadCreditsScene);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadCreditsScene()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Jeu fermé"); // Pour tester dans l'éditeur
    }
}