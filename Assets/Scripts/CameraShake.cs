// CameraShake.cs - Effet de secousse de caméra
using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPosition;
    private bool isShaking = false;
    
    void Start()
    {
        originalPosition = transform.localPosition;
    }
    
    public void ShakeCamera(float duration, float magnitude)
    {
        if (!isShaking)
        {
            StartCoroutine(Shake(duration, magnitude));
        }
    }
    
    IEnumerator Shake(float duration, float magnitude)
    {
        isShaking = true;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            
            transform.localPosition = originalPosition + new Vector3(x, y, 0);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.localPosition = originalPosition;
        isShaking = false;
    }
}