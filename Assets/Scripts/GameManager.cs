using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Interface")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI collectiblesText;
    
    [Header("Panneaux de fin")]
    public GameObject gameOverPanel;
    public GameObject victoryPanel;
    public Button restartButton;
    public Button menuButton;
    
    [Header("Configuration")]
    public float gameTime = 45f;
    public int totalCollectibles = 5;
    
    private float currentTime;
    private int collectedItems = 0;
    private bool gameEnded = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        currentTime = gameTime;
        UpdateUI();
        
        restartButton.onClick.AddListener(RestartGame);
        menuButton.onClick.AddListener(BackToMenu);
        
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
    }

    void Update()
    {
        if (!gameEnded)
        {
            currentTime -= Time.deltaTime;
            UpdateUI();
            
            if (currentTime <= 0)
            {
                GameOver();
            }
        }
    }

    public void CollectItem()
    {
        if (gameEnded) return;
        
        collectedItems++;
        UpdateUI();
        
        if (collectedItems >= totalCollectibles)
        {
            Victory();
        }
    }

    void UpdateUI()
    {
        timerText.text = "Temps: " + Mathf.Ceil(currentTime).ToString();
        collectiblesText.text = $"Objets: {collectedItems}/{totalCollectibles}";
    }

    void GameOver()
    {
        gameEnded = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void Victory()
    {
        gameEnded = true;
        victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }
}