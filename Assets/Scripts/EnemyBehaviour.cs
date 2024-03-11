using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public enum BEHAVIOURS {RUSH, SHOOT};
    public BEHAVIOURS behaviour;
    GameObject player;
    public GameObject bullet;
    Rigidbody2D rb;
    public int damage;

    public float speed;
    public float bulletSpeed;
    public float attackSpeed;
    public bool canAttack;
    // Start is called before the first frame update
    void Start()
    {
        canAttack = true;
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        lookToPlayer();
        if(canAttack)
            action();
    }

    void action(){
        switch( behaviour ){
            case BEHAVIOURS.RUSH:
                walkToPlayer();
                break;
            case BEHAVIOURS.SHOOT:
                shootAtPlayer();
                break;
        }
    }

    void shootAtPlayer(){
        if(Vector2.Distance(transform.position, player.transform.position) > 8){
            transform.position = Vector2.MoveTowards(
                transform.position, player.transform.position, speed * Time.deltaTime);
        }else{
            StartCoroutine(shoot());
        }
    }

    void walkToPlayer(){
        transform.position = Vector2.MoveTowards(
                transform.position, player.transform.position, speed * Time.deltaTime);
        speed += Time.deltaTime;
    }
    void lookToPlayer()
    {
        // Some math just to look at the player
        float m = (player.transform.position.y - transform.position.y) 
                                            /
                    (player.transform.position.x - transform.position.x);
        float angle = Mathf.Atan(m) * Mathf.Rad2Deg - 90;
        if(player.transform.position.x < transform.position.x)
            angle += 180;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    IEnumerator shoot(){
        canAttack = false;
        var b = Instantiate(bullet, transform.position, transform.rotation);
        b.GetComponent<EnemyShootScript>().bulletSpeed = bulletSpeed;
        yield return new WaitForSeconds(attackSpeed);
        canAttack = true;
    }
    IEnumerator attacked(GameObject player){
        canAttack = false;
        player.GetComponent<PlayerStats>().TakeDamage(damage, Vector2.zero);
        yield return new WaitForSeconds(attackSpeed);
        canAttack = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player"){
            speed = 5;
            var forceDir = (transform.position - collision.transform.position).normalized;
            var force = new Vector2(forceDir.x, forceDir.y) * 5;
            rb.AddForce(force, ForceMode2D.Impulse);
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            playerRb.AddForce(-force, ForceMode2D.Impulse);
            StartCoroutine(attacked(collision.gameObject));
        }
    }
}
