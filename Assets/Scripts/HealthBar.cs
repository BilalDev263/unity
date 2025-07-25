// HealthBar.cs - Barre de vie pour les ennemis
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Health Bar")]
    public Slider healthSlider;
    public Gradient healthGradient;
    public Image fillImage;
    
    private int maxHealth;
    private Camera playerCamera;
    
    void Start()
    {
        playerCamera = Camera.main;
    }
    
    void LateUpdate()
    {
        // Faire face à la caméra
        if (playerCamera)
        {
            transform.LookAt(transform.position + playerCamera.transform.rotation * Vector3.forward,
                           playerCamera.transform.rotation * Vector3.up);
        }
    }
    
    public void SetMaxHealth(int health)
    {
        maxHealth = health;
        healthSlider.maxValue = health;
        healthSlider.value = health;
        
        if (fillImage)
            fillImage.color = healthGradient.Evaluate(1f);
    }
    
    public void SetHealth(int health)
    {
        healthSlider.value = health;
        
        if (fillImage)
        {
            float normalizedHealth = (float)health / maxHealth;
            fillImage.color = healthGradient.Evaluate(normalizedHealth);
        }
    }
}