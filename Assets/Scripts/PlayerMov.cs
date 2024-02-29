using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Player input axis
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        
        //Move the player
        rb.velocity = new Vector2(x, y) * speed;

        // Rotate looking at the mouse
        Vector3 mousePos = Input.mousePosition;
        //Screen to world
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;
        transform.up = mousePos - transform.position;


    }

    // All Physic related stuff eventually
    void FixedUpdate() {

    }
}
