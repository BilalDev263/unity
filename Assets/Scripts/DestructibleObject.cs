// DestructibleObject.cs - Objets destructibles
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [Header("Destructible Settings")]
    public int maxHealth = 50;
    public GameObject destroyEffect;
    public AudioClip destroySound;
    public bool dropItem = false;
    public GameObject itemToDrop;
    
    private int currentHealth;
    
    void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            DestroyObject();
        }
    }
    
    void DestroyObject()
    {
        // Effet de destruction
        if (destroyEffect)
        {
            Instantiate(destroyEffect, transform.position, transform.rotation);
        }
        
        // Son de destruction
        if (destroySound)
        {
            AudioSource.PlayClipAtPoint(destroySound, transform.position);
        }
        
        // Drop d'objet
        if (dropItem && itemToDrop)
        {
            Instantiate(itemToDrop, transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }
}