using UnityEngine;

public class KamehamehaProjectile : MonoBehaviour
{
    [Header("Settings")]
    public int damage = 50;
    public float lifetime = 5f;
    
    void Start()
    {
        Debug.Log("💥 Kamehameha projectile créé !");
        
        // Auto-destruction après le lifetime
        Destroy(gameObject, lifetime);
    }
    
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("🔥 Kamehameha collision avec : " + other.name);
        
        // Ignorer le joueur qui l'a tiré
        if (other.CompareTag("Player")) return;
        
        // Si c'est un ennemi
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy)
            {
                enemy.TakeDamage(damage);
                Debug.Log("💥 Ennemi touché par Kamehameha projectile ! Dégâts: " + damage);
            }
        }
        
        // Détruire le projectile après impact
        Destroy(gameObject);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("🔥 Kamehameha collision (physics) avec : " + collision.gameObject.name);
        
        // Même logique que OnTriggerEnter
        if (collision.gameObject.CompareTag("Player")) return;
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            if (enemy)
            {
                enemy.TakeDamage(damage);
                Debug.Log("💥 Ennemi touché par Kamehameha projectile ! Dégâts: " + damage);
            }
        }
        
        Destroy(gameObject);
    }
}