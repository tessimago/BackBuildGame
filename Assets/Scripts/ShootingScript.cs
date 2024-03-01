using System.Collections;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    public GameObject bullet;
    public bool canShoot = true;
    public float shootDelay = 0.5f;
    public float spreadVar = 5;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
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
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }
}
