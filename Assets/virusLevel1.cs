using UnityEngine;

public class virusLevel1 : Virus
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        Debug.Log("health" + Health);
    }

    // Update is called once per frame
    void Update()
    {
        void Update()
        {
            // Example: decrease health every second
            Health -= Time.deltaTime / 2;
            Debug.Log("health" + Health);

            if (Health <= 0)
            {
                Death();
            }
        }

    }

    public override void Death()
    {
        throw new System.NotImplementedException();
    }

    public override void Shooting()
    {
        throw new System.NotImplementedException();
    }

}
