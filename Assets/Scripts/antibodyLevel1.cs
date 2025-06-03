using UnityEngine;

public class antibodyLevel1 : Antibody
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        // Example: decrease health every second
        Health -= Time.deltaTime / 2;

        if (Health <= 0)
        {
            Death();
        }

        transform.Translate(transform.forward * Speed * Time.deltaTime);

    }

    public override void Death()
    {
        Debug.Log("death");
        Destroy(gameObject);
        GameManager.instance.GainExp();

    }

    public override void Shooting()
    {
        Debug.Log("shoot");
    }
}
