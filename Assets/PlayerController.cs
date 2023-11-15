using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private float moveX;
    private float moveY;
    private bool keyControl;
    private bool safe;

    private Rigidbody2D rb;
    private Camera mainCam;
    private Vector3 mousePos;

    public GameObject shadow;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        keyControl = false;
        safe = true;
    }

    // Update is called once per frame
    void Update()
    {
        mainCam.transform.position = new Vector3(rb.position.x, rb.position.y, mainCam.transform.position.z);
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        shadow.transform.position = new Vector3(transform.position.x, transform.position.y-0.3f, shadow.transform.position.z);

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        
        if(HasMouseMoved()){
            keyControl = false;
        }

        if((vertical != 0f) || (horizontal != 0f)){
            keyControl = true;
            Vector3 keyVector = new Vector3(horizontal, vertical, 0);
            float keyRotation = Mathf.Atan2(keyVector.x, keyVector.y)*Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, -keyRotation);
            rb.velocity = new Vector2(keyVector.x, keyVector.y).normalized * speed;
            shadow.transform.rotation = Quaternion.Euler(0, 0, -keyRotation);
        } else if (!keyControl) {
            Vector3 rotation = mousePos - transform.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) *  Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
            rb.velocity = new Vector2(rotation.x, rotation.y).normalized * speed;
            shadow.transform.rotation = Quaternion.Euler(0, 0, rotZ);
        }

        //mouse control
        // mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        // Vector3 rotation = mousePos - transform.position;
        // float rotZ = Mathf.Atan2(rotation.y, rotation.x) *  Mathf.Rad2Deg;
        // transform.rotation = Quaternion.Euler(0, 0, rotZ);
        // rb.velocity = new Vector2(rotation.x, rotation.y).normalized * speed;

        //keys control
        
        // float keyRotation = Mathf.Atan2(horizontal, vertical)*Mathf.Rad2Deg;
        // transform.rotation = Quaternion.Euler(0, 0, keyRotation);


        //shadow control
        // shadow.transform.position = new Vector3(transform.position.x, transform.position.y-0.3f, shadow.transform.position.z);
        // shadow.transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    bool HasMouseMoved()
    {
        return (Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Debug.Log("TRIGGER COLLISION");
        // Debug.Log(collider);
        if(collider.name == "Base"){
            // Debug.Log("BASED");
            safe = true;
            //update base OR Base update handled by basecontroller
            // Debug.Log("Update Base");
        }
        
        if(collider.name == "TrailCollider"){
            // Debug.Log("TRAILED");
            if(!safe){
                //kill
                // Debug.Log("KILL");
            }
        }
        
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.name == "Base"){
            safe = false;
        }
    }
}
