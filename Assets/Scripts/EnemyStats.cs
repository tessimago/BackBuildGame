using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyStats : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 100;
    public int damage = 10;
    public int speed = 10;
    public int score = 10;
    
    // UI
    Slider healthBar;

    // Physics
    Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponentInChildren<Slider>();
        healthBar.maxValue = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int damage, Vector2 knockback){
        health -= damage;
        healthBar.value = health;
        // Apply a knockback force to the enemy
        rb.AddForce(knockback, ForceMode2D.Impulse);
        if(health <= 0){
            Destroy(gameObject);
        }
    }
}
