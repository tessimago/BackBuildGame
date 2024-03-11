
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

    public TextMeshProUGUI bestScoreText;
    public int score;
    public int bestScore;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.None;
        score = 0;
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
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
        if(score > bestScore){
            bestScore = score;
        }
    }
    public void TakeDamage(int damage, Vector2 knockback){
        health -= damage;
        // Apply a knockback force to the enemy
        rb.AddForce(knockback, ForceMode2D.Impulse);
        if(health <= 0){
            GameOver();
            health = 0;
        }
        updateBar();
    }
    public void Heal(int heal){
        health += heal;
        health = Mathf.Min(health, maxHealth);
        updateBar();
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
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        GameManager.gameState = GameManager.GAME_STATE.GAME_OVER;
        gameOverScreen.SetActive(true);
        bestScoreText.text = "Best Score: " + bestScore;
        PlayerPrefs.SetInt("BestScore", score);
    }

}
