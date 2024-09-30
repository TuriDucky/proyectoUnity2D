using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float xSpeed;
    float ySpeed;
    Rigidbody2D rb2D;
    
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A)){
            rb2D.velocity = new Vector2(-xSpeed, rb2D.velocity.y);
        }
        else if (Input.GetKey(KeyCode.D)){
            rb2D.velocity = new Vector2(xSpeed, rb2D.velocity.y);
        }
    }
}
