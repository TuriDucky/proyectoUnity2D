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
    public bool facingRight;

    public float jumptime;
    public float jumptimer;
    public bool isJumping;

    public char lastKeyPressed;


    public bool isDashing;
    public float dashTime;
    public float dashTimer;
    public int dashCount;
    public int dashCounter;

    Rigidbody2D rb2D;
    SpriteRenderer sr;
    
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        facingRight = true;
    }

    void Update(){
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow)){
            jumptimer = 0;
            isJumping = false;
        }

        if (!isDashing){

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
                lastKeyPressed = 'a';
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
                lastKeyPressed = 'd';
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)){
                lastKeyPressed = 'w';
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)){
                lastKeyPressed = 's';
            }
        }

        if (GroundCheck.isGrounded){
            dashCounter = dashCount;
        }
        if (Input.GetMouseButtonDown(0) && dashCounter > 0){
            isDashing = true;
            dashTimer = dashTime;
            dashCounter --;
        }
        
    }

    void FixedUpdate()
    {

        if (rb2D.velocity.x == 0){
            xSpeed = 0;
        } 

        
        
        

        if (isDashing){
            xSpeed = 20;
            if (lastKeyPressed == 'a'){
                xSpeed = -xSpeed;
                rb2D.velocity = new Vector2(xSpeed, 0);
            }
            if(lastKeyPressed == 'd'){
                rb2D.velocity = new Vector2(xSpeed, 0);
            }
            if (lastKeyPressed == 'w'){
                float dashup = (xSpeed / 4) * 3;
                xSpeed = 0;
                rb2D.velocity = new Vector2(0, dashup);
            }
            if (lastKeyPressed == 's'){
                float dashup = -xSpeed;
                xSpeed = 0;
                rb2D.velocity = new Vector2(0, dashup);
            }
            
            dashTimer = dashTimer - 1;
            if (dashTimer == 0){
                isDashing = false;
                
            }
        }
        
        if (!isDashing){
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
                
                facingRight = false;
                sr.flipX = false;

                if (GroundCheck.isGrounded){
                    xSpeed = xSpeed - xAcceleration * 3;
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
            
            facingRight = true;
            sr.flipX = true;
            if (GroundCheck.isGrounded){
                xSpeed = xSpeed + xAcceleration * 3;
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

    
}
