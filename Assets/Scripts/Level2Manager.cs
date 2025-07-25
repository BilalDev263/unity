using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level2Manager : MonoBehaviour
{
    [Header("UI References - Optionnelles")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI enemiesText;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;
    
    [Header("Game Settings")]
    public int playerMaxHealth = 100;
    public int totalEnemies = 3; // Nombre d'ennemis à placer dans la scène
    
    private int currentPlayerHealth;
    private int enemiesDefeated;
    private bool gameActive = true;
    
    void Start()
    {
        currentPlayerHealth = playerMaxHealth;
        enemiesDefeated = 0;
        
        // Initialiser les UI seulement si elles existent
        UpdateHealthDisplay();
        UpdateEnemiesDisplay();
        
        // Masquer les panels seulement s'ils existent
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (victoryPanel) victoryPanel.SetActive(false);
        
        Debug.Log("Level2Manager démarré - Santé: " + currentPlayerHealth + " - Ennemis à vaincre: " + totalEnemies);
    }
    
    void Update()
    {
        // Plus d'inputs ici pour éviter les conflits avec le New Input System
        // Les tests peuvent être faits directement depuis GokuController
    }
    
    void UpdateHealthDisplay()
    {
        if (healthText)
            healthText.text = "Vie : " + currentPlayerHealth + "/" + playerMaxHealth;
    }
    
    void UpdateEnemiesDisplay()
    {
        if (enemiesText)
            enemiesText.text = "Ennemis : " + enemiesDefeated + "/" + totalEnemies;
    }
    
    public void TakeDamage(int damage)
    {
        if (!gameActive) return;
        
        currentPlayerHealth -= damage;
        if (currentPlayerHealth < 0) currentPlayerHealth = 0;
        UpdateHealthDisplay();
        
        Debug.Log("💔 Goku a pris " + damage + " dégâts. Santé restante: " + currentPlayerHealth);
        
        if (currentPlayerHealth <= 0)
        {
            GameOver();
        }
    }
    
    public void HealPlayer(int healAmount)
    {
        if (!gameActive) return;
        
        currentPlayerHealth += healAmount;
        if (currentPlayerHealth > playerMaxHealth) currentPlayerHealth = playerMaxHealth;
        UpdateHealthDisplay();
        
        Debug.Log("💚 Goku s'est soigné de " + healAmount + " PV. Santé: " + currentPlayerHealth);
    }
    
    public void EnemyDefeated()
    {
        if (!gameActive) return;
        
        enemiesDefeated++;
        UpdateEnemiesDisplay();
        
        Debug.Log("🎯 Ennemi défait ! Progression: " + enemiesDefeated + "/" + totalEnemies);
        
        if (enemiesDefeated >= totalEnemies && totalEnemies > 0)
        {
            Victory();
        }
    }
    
    void GameOver()
    {
        gameActive = false;
        Debug.Log("💀 GAME OVER !");
        
        if (gameOverPanel) 
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    
    void Victory()
    {
        gameActive = false;
        Debug.Log("🏆 VICTOIRE ! Tous les ennemis sont vaincus !");
        
        if (victoryPanel) 
        {
            victoryPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    
    public bool IsGameActive()
    {
        return gameActive;
    }
}