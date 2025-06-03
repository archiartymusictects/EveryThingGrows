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
            // Example: decrease health every second
        Health -= Time.deltaTime / 2;
        Debug.Log("health" + Health);

        if (Health <= 0)
        {
            Death();
        }

        transform.Translate(transform.forward * Speed * Time.deltaTime);

    }

    public override void Death()
    {
        Debug.Log("death");
    }

    public override void Shooting()
    {
        Debug.Log("shoot");
    }

}
