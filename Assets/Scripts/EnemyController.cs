using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int maxHealth = 50;
    public int attackDamage = 10;
    
    [Header("Visual")]
    public Renderer enemyRenderer;
    
    private int currentHealth;
    private bool isDead = false;
    private Level2Manager gameManager;
    
    void Start()
    {
        currentHealth = maxHealth;
        
        // Chercher le game manager (peut être null)
        gameManager = FindObjectOfType<Level2Manager>();
        
        // Récupérer le renderer si pas assigné
        if (enemyRenderer == null)
            enemyRenderer = GetComponent<Renderer>();
        
        // Couleur rouge pour identifier l'ennemi
        if (enemyRenderer)
            enemyRenderer.material.color = Color.red;
        
        // S'assurer qu'il n'y a pas de NavMeshAgent
        UnityEngine.AI.NavMeshAgent navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (navAgent)
        {
            Debug.LogWarning("⚠️ NavMeshAgent détecté sur " + gameObject.name + " - Le retirer pour éviter les erreurs");
            navAgent.enabled = false;
        }
            
        Debug.Log("👹 Ennemi créé avec " + maxHealth + " PV");
    }
    
    void Update()
    {
        // Rien dans Update pour éviter les erreurs
        // Les interactions se feront par les autres méthodes
    }
    
    public void TakeDamage(int damage)
    {
        if (isDead) return;
        
        currentHealth -= damage;
        Debug.Log("💔 Ennemi prend " + damage + " dégâts. Santé restante: " + currentHealth);
        
        // Effet visuel simple : clignotement blanc
        if (enemyRenderer)
        {
            StartCoroutine(FlashWhite());
        }
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        if (isDead) return;
        isDead = true;
        
        Debug.Log("💀 Ennemi défait !");
        
        // Informer le game manager si il existe
        if (gameManager)
        {
            gameManager.EnemyDefeated();
        }
        
        // Effet visuel de mort
        if (enemyRenderer)
        {
            enemyRenderer.material.color = Color.gray;
        }
        
        // Désactiver les colliders
        Collider col = GetComponent<Collider>();
        if (col) col.enabled = false;
        
        // Disparaître après 2 secondes
        Destroy(gameObject, 2f);
    }
    
    System.Collections.IEnumerator FlashWhite()
    {
        if (enemyRenderer == null) yield break;
        
        Color originalColor = enemyRenderer.material.color;
        enemyRenderer.material.color = Color.white;
        
        yield return new WaitForSeconds(0.1f);
        
        if (enemyRenderer && !isDead) // Vérifier qu'il existe encore et n'est pas mort
            enemyRenderer.material.color = originalColor;
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Si le joueur touche l'ennemi, il prend des dégâts
        if (other.CompareTag("Player") && !isDead)
        {
            GokuController goku = other.GetComponent<GokuController>();
            if (goku)
            {
                goku.TakeDamage(attackDamage);
                Debug.Log("⚔️ Ennemi attaque Goku pour " + attackDamage + " dégâts !");
            }
        }
    }
}