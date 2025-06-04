using System.Collections;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class TugOfWarHealth : MonoBehaviour
{
    public Slider TugOfWarHealthBar; // Assign in Inspector
    public float currentTugOfWarHealth = 50; // Start at midpoint (50/100)
    public float smoothSpeed = 5f;         // How fast the bar animates
    public TextMeshProUGUI resultText; // Assign in Inspector for game over messages
    private bool isGameOver = false; // Track if game is over

    [SerializeField] Slider audioSlider;

    void Start()
    {
        TugOfWarHealthBar.value = currentTugOfWarHealth; // Initialize the slider
    }
    void Update()
    {     // health changing test
        if (Input.GetKeyDown(KeyCode.X))
            PlayerDamaged(10); // Press X: Player loses 10 HP (bar moves left)

        if (Input.GetKeyDown(KeyCode.C))
            EnemyDamaged(10); // Press C: Enemy loses 10 HP (bar moves right)

        /*  if (currentTugOfWarHealth <= 0)
              Debug.Log("Player loses!"); // Enemy wins

          if (currentTugOfWarHealth >= 100)
              Debug.Log("Player wins!"); // Enemy loses */
        TugOfWarHealthBar.value = Mathf.Lerp(TugOfWarHealthBar.value, currentTugOfWarHealth, smoothSpeed * Time.deltaTime);

    }

    // Call this when PLAYER takes damage (moves left)
    public void PlayerDamaged(float damage)
    {
        if (isGameOver) return; // Don't process damage if game already ended
        currentTugOfWarHealth = Mathf.Max(0, currentTugOfWarHealth - damage);
        /*TugOfWarHealthBar.value = currentTugOfWarHealth;*/ // This line is now in Update() for smooth animation
        if (currentTugOfWarHealth <= 0) StartCoroutine(CheckGameOverCoroutine());
    }

    // Call this when ENEMY takes damage (moves right)
    public void EnemyDamaged(float damage)
    {
        if (isGameOver) return; // Don't process damage if game already ended
        currentTugOfWarHealth = Mathf.Min(100, currentTugOfWarHealth + damage);
        /*TugOfWarHealthBar.value = currentTugOfWarHealth;*/ // This line is now in Update() for smooth animation
        if (currentTugOfWarHealth >= 100) StartCoroutine(CheckGameOverCoroutine());

        audioSlider.value += 10;
    }

    /* void CheckGameOver()
     {
         if (currentTugOfWarHealth <= 0)
         {
             Debug.Log("Player loses!"); // Enemy wins
             resultText.text = "DEFEAT!";
             resultText.color = Color.red;
             Time.timeScale = 0f; // Freeze game

         }
         else if (currentTugOfWarHealth >= 100)
         {
             Debug.Log("Player wins!"); // Enemy loses
             resultText.text = "VICTORY!";
             resultText.color = Color.green;
             Time.timeScale = 0f; // Freeze game 
         }*/

    IEnumerator CheckGameOverCoroutine()
    {
        
        // Wait until health bar finishes animating
        while (Mathf.Abs(TugOfWarHealthBar.value - currentTugOfWarHealth) > 0.1f)
        {
            yield return null;
        }

        if (currentTugOfWarHealth <= 0 && !isGameOver)
        {
            isGameOver = true;
            
            resultText.text = "DEFEAT!";
            resultText.color = Color.red;
            resultText.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (currentTugOfWarHealth >= 100 && !isGameOver)
        {
            
            resultText.text = "VICTORY!";
            resultText.color = Color.green;
            resultText.gameObject.SetActive(true);
            Time.timeScale = 0f; // Freeze game
        }
    }
}