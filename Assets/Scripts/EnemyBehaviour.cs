using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
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
}
