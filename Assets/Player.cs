using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;
    Rigidbody2D rb2D;
    
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
            rb2D.velocity = new Vector2(-xSpeed, rb2D.velocity.y);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
            rb2D.velocity = new Vector2(xSpeed, rb2D.velocity.y);
        }

        if ( GroundCheck.grounded == true){
            if(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow)){
            rb2D.velocity = new Vector2(rb2D.velocity.x, ySpeed);
        }
        }
        

        
    }

    
}
