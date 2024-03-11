using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootScript : MonoBehaviour
{
    public float bulletSpeed = 25f;
    int bulletDamage = 15;
    float knockbackForce = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * Time.deltaTime * bulletSpeed);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
        if(other.gameObject.tag == "Player"){
            Destroy(gameObject);
            // Get enemy ui
            Vector2 knockbackDirection = transform.forward.normalized;
            other.gameObject.GetComponent<PlayerStats>().TakeDamage(bulletDamage, knockbackDirection * knockbackForce);
        }
    }
}
