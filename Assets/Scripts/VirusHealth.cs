using UnityEngine;
using UnityEngine.UI;

public class VirusHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Slider HealthBar; // Assign in Inspector

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (healthBar != null)
            healthBar.value = currentHealth;

        if (currentHealth <= 0)
            Destroy(gameObject);
    }
}