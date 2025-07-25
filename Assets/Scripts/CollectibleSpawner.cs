using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    public GameObject collectiblePrefab;
    public int numberToSpawn = 10;

    // Position réelle du centre du sol
    private Vector3 center = new Vector3(385.76f, 0f, -45.14f);

    // Taille du sol (Plane scale 5 = 50 unités)
    private float areaWidth = 50f;
    private float areaDepth = 50f;

    void Start()
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(center.x - areaWidth / 2f, center.x + areaWidth / 2f),
                center.y + 1f,
                Random.Range(center.z - areaDepth / 2f, center.z + areaDepth / 2f)
            );

            Instantiate(collectiblePrefab, randomPosition, Quaternion.identity);
        }
    }
}
