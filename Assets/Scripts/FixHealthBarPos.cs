using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixHealthBarPos : MonoBehaviour
{
    Camera cam;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.rotation = cam.transform.rotation;
        transform.position = target.position + new Vector3(0, 1, 0);
    }
}
