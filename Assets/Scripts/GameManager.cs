using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int tugOfWarHealth;

    //Player variables
    private float currency;
    private float currencyGrowthRate = 0.1f;
    private float exp;
    private int level;
    [SerializeField] private GameObject player; 


    [SerializeField] private TextMeshProUGUI gameWinText; //could switch these to a gameobject prefab to do a whole game win/lose screen
    [SerializeField] private TextMeshProUGUI gameOverText;
    


    private void Start()
    {
        instance = this;

        tugOfWarHealth = 50; 
        exp = 0; 
        level = 1;
        currency = 100;

        //subscribe to events here?
     
    }

    private void Update()
    {
        

        

    }

    private void FixedUpdate()
    {
        currency += currencyGrowthRate; //player currency generates slowly over time, maybe adjust rate when virus grows? 
    }

    void ChangeCurrency(int change)
    {
        currency += change;
    }    

    void GameWin()
    {
        gameWinText.enabled = true;
    }

    void GameOver()
    {
        gameOverText.enabled = true;
    }

    private void Grow()
    {
        level++;

        // logic for virus growth here, keep it simple and change scale?
        player.transform.localScale = new Vector3(level, level, level);
        
    }

    private void GainExp()
    {
        exp++;
        if (exp >= 100)
        {
            Grow();
            exp = 0;
        }
    }

    private void PlayerTakeDamage(int damage)
    {
        tugOfWarHealth -= damage;

        if (tugOfWarHealth <= 0)
        {
            GameOver();
        }
    }

    private void EnemyTakeDamage(int damage)
    {
        tugOfWarHealth += damage;

        if (tugOfWarHealth >= 100)
        {
            GameWin();
        }

    }
}
