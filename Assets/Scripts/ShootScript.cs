using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    public AudioClip shootSound;
    public AudioSource speaker;
    public float speed = 25f;
    public int bulletDamage = 15;
    public float knockbackForce = 2f;
    GameObject player;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        transform.localScale = new Vector3( transform.localScale.x,
                                            Mathf.Max(transform.localScale.y+((speed-25)/100)),
                                            transform.localScale.z);
        transform.position += transform.up * ((speed-25)/200);
        speaker = GameObject.Find("SoundManager").GetComponent<AudioSource>();
        speaker.PlayOneShot(shootSound);
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector2.up * Time.deltaTime * speed);
    }
    void FixedUpdate() {
        rb.velocity = (transform.up * speed);    
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
            other.gameObject.GetComponent<EnemyStats>().TakeDamage(bulletDamage, knockbackDirection * knockbackForce);
        }
    }
}
