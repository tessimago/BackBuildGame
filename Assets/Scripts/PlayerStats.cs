
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject pauseScreen;
    public Image healthBar;
    public TextMeshProUGUI healthText;
    public Image xpBar;
    public TextMeshProUGUI levelText;
    public GameObject panelLevelUp;
    public int xp;
    public int xpToNextLevel;
    public int level;
    public int health;
    public int maxHealth;
    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI bestScoreText;
    public int score;
    public int bestScore;
    public Rigidbody2D rb;
    public GameManager gameManag;
    // Start is called before the first frame update
    void Start()
    {
        gameManag = GameObject.Find("GameManager").GetComponent<GameManager>();
        level = 1;
        levelText.text = "Level: " + level;
        xp = 0;
        xpToNextLevel = 100;
        gameManag.Resume();
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
        if(GameManager.gameState != GameManager.GAME_STATE.GAME)
            return;

        score = Mathf.Max((int) transform.position.x - 10, score);
        scoreText.text = "Score: " + score;
        if(score > bestScore){
            bestScore = score;
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            //AddXp(10);
        }
        if(Input.GetKeyDown(KeyCode.P)){
            Pause();
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
    void LevelUp(){
        gameManag.Pause();
        panelLevelUp.SetActive(true);
        LevelUpPanel levelUpPanel = panelLevelUp.GetComponent<LevelUpPanel>();
        levelUpPanel.RandomizeOptions();
    }
    public void AddXp(int xpReceived){
        var currXp = xp;
        Debug.Log("Received xp: " + xpReceived);
        xp += xpReceived;
        if(xp >= xpToNextLevel){ // Nao preciso repetir enquanto tiver a subir de nivel, nenhum xp dá tanto assim
            level++;
            xp -= xpToNextLevel;
            xpToNextLevel += 100;
            levelText.text = "Level: " + level;
            LevelUp();
        }
        if(xpMov != null)
            StopCoroutine(xpMov);
        xpMov = StartCoroutine(xpBarMotion(currXp, xp, 0.2f));
        
    }
    Coroutine xpMov;

    IEnumerator xpBarMotion(int initialXP, int targetXp, float delay){
        float time = 0;
        while(time < delay){
            xpBar.fillAmount = Mathf.Lerp((float)initialXP / xpToNextLevel, (float)targetXp / xpToNextLevel, time/delay);
            time += Time.deltaTime;
            yield return null;
        }
    }
    public void Heal(int heal){
        health += heal;
        health = Mathf.Min(health, maxHealth);
        updateBar();
    }
    public void updateBar(){
        healthBar.fillAmount = (float)health / maxHealth;
        healthText.text = health + "/" + maxHealth;
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

    public void Pause(){
        gameManag.Pause();
        pauseScreen.SetActive(true);
    }

    public void Resume(){
        pauseScreen.SetActive(false);
    }

}
