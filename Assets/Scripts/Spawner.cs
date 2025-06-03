using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject cellToSpawn;
    public GameObject spawnPoint;
    public Vector3 spawnPosition; 
    public Quaternion spawnRotation;

    public int spawnRate;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPosition = spawnPoint.transform.position;
        spawnRotation = spawnPoint.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnRate != 0)
        {
            if (Time.frameCount % spawnRate == 0)
            {
                Spawn();

            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && gameObject.CompareTag("Player") && GameManager.instance.GetCurrency() >= 20)
        {
            Spawn();
            GameManager.instance.ChangeCurrency(-20);
        }
    }

    void Spawn()
    {
        GameObject newObject = Instantiate(cellToSpawn, spawnPosition, spawnRotation);


    }
}
