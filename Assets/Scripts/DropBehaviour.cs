using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBehaviour : MonoBehaviour
{   
    GameObject player;
    Rigidbody2D rb;
    float speed = 3;
    [SerializeField] int xpGive = 5;
    public AudioClip xpSound;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        //Get random vector2
        Vector2 randomVector = Random.insideUnitCircle;
        // Explosion force to rb
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(randomVector, ForceMode2D.Impulse);
        Destroy(gameObject, 60);
    }

    public void Initialize(int xp){
        xpGive = xp;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, player.transform.position) < 3){
            //rb.AddForce(speed * (player.transform.position - transform.position));
            transform.position = Vector2.MoveTowards(
                transform.position, player.transform.position, speed * Time.deltaTime);
            speed += 0.1f;
        }else{
            speed = 3;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && GameManager.gameState == GameManager.GAME_STATE.GAME){
            other.GetComponent<PlayerStats>().Heal(1);
            other.GetComponent<PlayerStats>().AddXp(xpGive);
            GameObject.Find("SoundManager").GetComponent<AudioSource>().PlayOneShot(xpSound);
            Destroy(gameObject);
        }
    }
    
}
