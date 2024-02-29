using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    float speed = 25f;
    int bulletDamage = 15;
    float knockbackForce = 2f;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.Find("Player");
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * Time.deltaTime * speed);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
        if(other.gameObject.tag == "Enemy"){
            Destroy(gameObject);
            // Get enemy ui
            Vector2 knockbackDirection = (other.transform.position - player.transform.position).normalized;
            other.gameObject.GetComponent<EnemyStats>().takeDamage(bulletDamage, knockbackDirection * knockbackForce);
        }
    }
}
