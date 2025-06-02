using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;         // Total health
    private int currentHealth;           // Current health
    public Slider healthBar;             // Reference to the UI Slider

    void Start()
    {
        currentHealth = maxHealth;       // Initialize health
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;  // Set slider max
            healthBar.value = currentHealth; // Update slider
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;         // Reduce health
        if (healthBar != null)
            healthBar.value = currentHealth; // Update slider

        if (currentHealth <= 0)
            Destroy(gameObject);         // Die if health ≤ 0
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            TakeDamage(10); // Press SPACE to deal 10 damage
    }
}