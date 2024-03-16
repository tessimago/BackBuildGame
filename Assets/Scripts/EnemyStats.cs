using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyStats : MonoBehaviour
{
    int health;
    int maxHealth;
    
    // UI
    Slider healthBar;

    // Physics
    Rigidbody2D rb;

    // Drops
    public GameObject drop;
    int xpDrop;
    
    
    // Start is called before the first frame update
    void Start()
    {
    }

    public void Initialize(int maxH, int xpDrop){
        healthBar = GetComponentInChildren<Slider>();
        rb = GetComponent<Rigidbody2D>();
        maxHealth = maxH;
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
        health = maxHealth;
        this.xpDrop = xpDrop;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage, Vector2 knockback){
        health -= damage;
        healthBar.value = health;
        // Apply a knockback force to the enemy
        rb.AddForce(knockback, ForceMode2D.Impulse);
        if(health <= 0){
            for(int i = 0; i < 3; i++){
                var d = Instantiate(drop, transform.position, Quaternion.identity);
                d.GetComponent<DropBehaviour>().Initialize(xpDrop);
            }
            Destroy(gameObject);
        }
    }
}
