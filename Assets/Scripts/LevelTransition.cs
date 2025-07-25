using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LevelTransition : MonoBehaviour
{
    [Header("Transition Settings")]
    public string nextSceneName = "Level2";
    public float transitionDelay = 1f;
    
    [Header("UI Elements")]
    public GameObject transitionUI;
    public TextMeshProUGUI transitionText;
    public CanvasGroup fadeCanvasGroup;
    
    [Header("Effects")]
    public ParticleSystem transitionEffect;
    public AudioClip transitionSound;
    
    private bool hasTriggered = false;
    
    void Start()
    {
        // S'assurer que l'UI de transition est masquée au début
        if (transitionUI)
            transitionUI.SetActive(false);
            
        if (fadeCanvasGroup)
            fadeCanvasGroup.alpha = 0f;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        
        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(TriggerTransition());
        }
    }
    
    IEnumerator TriggerTransition()
    {
        Debug.Log("Transition vers le niveau suivant...");
        
        // Afficher l'UI de transition
        if (transitionUI)
            transitionUI.SetActive(true);
            
        if (transitionText)
            transitionText.text = "Niveau suivant...";
        
        // Effet sonore
        if (transitionSound)
            AudioSource.PlayClipAtPoint(transitionSound, transform.position);
        
        // Effet de particules
        if (transitionEffect)
            transitionEffect.Play();
        
        // Fade out
        if (fadeCanvasGroup)
        {
            float fadeTime = 1f;
            float elapsedTime = 0f;
            
            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.deltaTime;
                fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeTime);
                yield return null;
            }
        }
        
        // Attendre le délai
        yield return new WaitForSeconds(transitionDelay);
        
        // Charger la scène suivante
        SceneManager.LoadScene(nextSceneName);
    }
    
    // Méthode pour déclencher manuellement la transition (ex: depuis un bouton)
    public void TriggerManualTransition()
    {
        if (!hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(TriggerTransition());
        }
    }
}