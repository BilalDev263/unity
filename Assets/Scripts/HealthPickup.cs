// HealthPickup.cs - Objet de soin
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public int healAmount = 25;
    public AudioClip pickupSound;
    public GameObject pickupEffect;
    
    void Start()
    {
        // Rotation lente pour attirer l'attention
        StartCoroutine(RotatePickup());
    }
    
    System.Collections.IEnumerator RotatePickup()
    {
        while (true)
        {
            transform.Rotate(0, 90 * Time.deltaTime, 0);
            yield return null;
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Level2Manager gameManager = FindObjectOfType<Level2Manager>();
            if (gameManager)
            {
                gameManager.HealPlayer(healAmount);
            }
            
            // Effet sonore
            if (pickupSound)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            
            // Effet visuel
            if (pickupEffect)
                Instantiate(pickupEffect, transform.position, transform.rotation);
            
            Destroy(gameObject);
        }
    }
}