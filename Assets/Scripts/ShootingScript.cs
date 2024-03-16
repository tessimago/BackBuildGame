using System.Collections;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    public GameObject bullet;
    public bool canShoot = true;
    public float shootDelay = 0.5f;
    public float spreadVar = 5;

    public int bulletDmg = 15;
    public float bulletSpeed = 20;
    public float bulletKnock = 2f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.gameState == GameManager.GAME_STATE.GAME_OVER)
            return;
            
        if(Input.GetMouseButton(0) && canShoot){
            StartCoroutine(ShootDelay());
        }
    }

    IEnumerator ShootDelay(){
        canShoot = false;
        float spread = Random.Range(-spreadVar, spreadVar);
        //Quaternion spreadQ = Quaternion.Euler(0, 0, spread);
        var b = Instantiate(bullet, transform.position, transform.rotation);
        b.transform.Rotate(0, 0, spread);
        SetBulletStats(b);
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }

    void SetBulletStats(GameObject b){
        b.GetComponent<ShootScript>().bulletDamage = bulletDmg;
        b.GetComponent<ShootScript>().speed = bulletSpeed;
        b.GetComponent<ShootScript>().knockbackForce = bulletKnock;
    }
}
