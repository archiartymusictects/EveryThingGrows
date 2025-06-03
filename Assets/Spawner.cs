using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject cellToSpawn;  
    public Vector3 spawnPosition = Vector3.zero; 
    public Quaternion spawnRotation = Quaternion.identity;

    public int spawnRate;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.frameCount % spawnRate == 0) 
        {
            spawn(); 
                
        }
    }

    void spawn()
    {
        GameObject newObject = Instantiate(cellToSpawn, spawnPosition, spawnRotation);


    }
}
