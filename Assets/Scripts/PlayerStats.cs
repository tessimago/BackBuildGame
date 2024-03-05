
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public GameObject gameOverScreen;
    public Image healthBar;
    public int health;
    public int maxHealth;
    public TextMeshProUGUI scoreText;
    public int score;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        score = 0;
        scoreText.text = "Score: " + score;
        maxHealth = 100;
        health = maxHealth;
        healthBar.fillAmount = 1;
        
        gameOverScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.gameState == GameManager.GAME_STATE.GAME_OVER)
            return;

        score = Mathf.Max((int) transform.position.x - 10, score);
        scoreText.text = "Score: " + score;
    }
    public void TakeDamage(int damage){
        health -= damage;
        if(health <= 0){
            GameOver();
            health = 0;
        }
        updateBar();

        // Overcomplicated code to go from green to yellow to red that does the same
        /*
        if(healthBar.fillAmount >= 0.5f){
        healthBar.color = new Vector4(2*(1-healthBar.fillAmount),
                                      healthBar.color.g,
                                      healthBar.color.b,
                                      1);
        }
        else{
            healthBar.color = new Vector4(healthBar.color.r,
                                          healthBar.fillAmount*2,
                                          healthBar.color.b,
                                          1);
        }
        */
    }
    void updateBar(){
        healthBar.fillAmount = (float)health / maxHealth;

        if(healthBar.fillAmount >= 0.5f)
            healthBar.color = Color.Lerp(Color.yellow, Color.green, healthBar.fillAmount/2);
        else{
            healthBar.color = Color.Lerp(Color.red, Color.yellow, healthBar.fillAmount*2);
		}
    }

    public void GameOver(){
        Time.timeScale = 0.1f;
        GameManager.gameState = GameManager.GAME_STATE.GAME_OVER;
        gameOverScreen.SetActive(true);
    }

}
