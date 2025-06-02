using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject cellToSpawn;  
    public Vector3 spawnPosition = Vector3.zero; 
    public Quaternion spawnRotation = Quaternion.identity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        spawn();
    }

    void spawn()
    {
        GameObject newObject = Instantiate(cellToSpawn, spawnPosition, spawnRotation);
    }
}
