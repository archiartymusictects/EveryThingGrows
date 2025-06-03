using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField] GameObject entityPrefab;
    [SerializeField] Transform spawnPosition;
    [SerializeField] float spawnTime = 5f;

    float spawnTimer;

    void Update()
    {
        spawnTimer += Time.deltaTime;

        SpawnEntity();
    }

    void SpawnEntity()
    {
        if (spawnTimer > spawnTime)
        {
            Instantiate(entityPrefab, spawnPosition.position, Quaternion.identity);
            spawnTimer = 0f;
        }
    }
}
