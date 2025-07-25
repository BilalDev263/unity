using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsManager : MonoBehaviour
{
    public Button backButton;

    void Start()
    {
        backButton.onClick.AddListener(BackToMenu);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}