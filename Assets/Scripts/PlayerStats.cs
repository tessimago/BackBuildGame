
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public Image healthBar;
    public int health;
    public int maxHealth;
    public TextMeshProUGUI scoreText;
    public int score;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        scoreText.text = "Score: " + score;
        maxHealth = 100;
        health = maxHealth;
        healthBar.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        score = Mathf.Max((int) transform.position.x - 10, score);
        scoreText.text = "Score: " + score;
    }
    public void TakeDamage(int damage){
        health -= damage;
        if(health <= 0){
            health = 0;
        }
        healthBar.fillAmount = (float)health / maxHealth;

        if(healthBar.fillAmount >= 0.5f)
            healthBar.color = Color.Lerp(Color.yellow, Color.green, healthBar.fillAmount/2);
        else{
            healthBar.color = Color.Lerp(Color.red, Color.yellow, healthBar.fillAmount*2);
		}

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
}
