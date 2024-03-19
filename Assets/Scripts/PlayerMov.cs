
using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 5f;
    float realSpeed;
    // For Keyboard
    float x;
    float y;
    // For Mouse
    Vector2 mouseDir;
    Camera mainCam;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        x = 0;
        y = 0;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.gameState != GameManager.GAME_STATE.GAME)
            return;
            
        realSpeed = speed * rb.drag;

        // Keyboard Input
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        // Mouse Input
        if(Input.GetMouseButton(1))
            mouseDir = mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        else
            mouseDir = Vector2.zero;
        mouseDir.Normalize();
        
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
        rb.AddForce(mouseDir * realSpeed, ForceMode2D.Force);
        
    }

    void speedControl(){
        if(rb.velocity.magnitude > speed)
            rb.velocity = rb.velocity.normalized * speed;
    }
}
