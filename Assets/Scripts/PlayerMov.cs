using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 5f;
    float realSpeed;
    float x;
    float y;
    // Start is called before the first frame update
    void Start()
    {
        x = 0;
        y = 0;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        realSpeed = speed * rb.drag;

        //Player input axis
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        speedControl();
        lookToMouse();
    }

    void lookToMouse(){
        // Rotate looking at the mouse
        Vector3 mousePos = Input.mousePosition;
        //Screen to world
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;
        transform.up = mousePos - transform.position;
    }

    // All Physic related stuff eventually
    void FixedUpdate() {
        //Move the player
        if(x != 0 || y != 0)
            rb.AddForce(new Vector2(x,y) * realSpeed, ForceMode2D.Force);
        
    }

    void speedControl(){
        if(rb.velocity.magnitude > speed)
            rb.velocity = rb.velocity.normalized * speed;
        
    }
}
