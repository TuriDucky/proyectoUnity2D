using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;
    public float xAcceleration;
    public float xSpeedCap;
    public float xSpeedCapAirborn;

    public float jumptime;
    public float jumptimer;
    public bool isJumping;

    Rigidbody2D rb2D;
    
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (rb2D.velocity.x == 0){
            xSpeed = 0;
        } 
        

        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
            if (GroundCheck.isGrounded){
                xSpeed = xSpeed - xAcceleration * 2;
                if (xSpeed < -xSpeedCap){
                    xSpeed = -xSpeedCap;
                }
            }
            else{
                if (xSpeed >= -xSpeedCapAirborn){
                    xSpeed = xSpeed - xAcceleration;

                    if (xSpeed < -xSpeedCapAirborn){
                        xSpeed = -xSpeedCapAirborn;
                    }
                
                }
            }
            rb2D.velocity = new Vector2(xSpeed, rb2D.velocity.y);
        }

        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
            if (GroundCheck.isGrounded){
                xSpeed = xSpeed + xAcceleration * 2;
                if (xSpeed > xSpeedCap){
                    xSpeed = xSpeedCap;
                }
            }
            
            else{
                if (xSpeed <= xSpeedCapAirborn){
                    xSpeed = xSpeed + xAcceleration;

                    if (xSpeed > xSpeedCapAirborn){
                        xSpeed = xSpeedCapAirborn;
                    }
            
                }
            }
            rb2D.velocity = new Vector2(xSpeed, rb2D.velocity.y);
        }
        else{
            if (GroundCheck.isGrounded){
                if (xSpeed != 0){
                    if (xSpeed > 0){
                        xSpeed = xSpeed - xAcceleration;
                        rb2D.velocity = new Vector2(xSpeed, rb2D.velocity.y);
                    }
                    else{
                        xSpeed = xSpeed + xAcceleration;
                        rb2D.velocity = new Vector2(xSpeed, rb2D.velocity.y);
                    }
                }
            }  
        }

        if (GroundCheck.isGrounded){
            if(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow)){
                jumptimer = jumptime;
                isJumping = true;
                rb2D.velocity = new Vector2(rb2D.velocity.x, ySpeed);
            }
            
        }

        if(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow)){
            if (jumptimer != 0){
                rb2D.velocity = new Vector2(rb2D.velocity.x, ySpeed);
                jumptimer = jumptimer - 1;
            }
            
        }
    }

    void Update(){
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow)){
            jumptimer = 0;
            isJumping = false;
        }
    }
}
