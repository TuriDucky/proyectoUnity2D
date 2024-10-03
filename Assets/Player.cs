using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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

    public float slashDelay;
    public float slashDelayCounter;
    public bool isHit;

    public float knockbackTimeCounter;
    public float knockbackTime;

    public GameObject SlashPrefab;

    Rigidbody2D rb2D;
    SpriteRenderer sr;
    
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        facingRight = true;
        isHit = false;
    }

    void Update(){
        if(slashDelayCounter > 0){
            slashDelayCounter --;
        }

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
        if (Input.GetMouseButtonDown(1) && dashCounter > 0){
            slashDelayCounter = slashDelay - 10;
            slash();
            isDashing = true;
            dashTimer = dashTime;
            dashCounter --;
        }

        if (Input.GetMouseButtonDown(0) && slashDelayCounter <= 0){
            slashDelayCounter = slashDelay;
            slash();
        }

        
        
    }

    void FixedUpdate()
    {
        if (isHit){
            knockbackTimeCounter --;
            if (knockbackTimeCounter == 0){
                isHit = false;
                rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            }
        }

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
        
        if (!isDashing && !isHit){
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
                            xSpeed = xSpeed - xAcceleration * 5;
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

    void slash(){
        GameObject slash = Instantiate(SlashPrefab, transform.position, Quaternion.identity);
        slash.transform.parent = gameObject.transform;
            if (lastKeyPressed == 'a'){
                slash.transform.Rotate(0,0,-90);
            }
            if(lastKeyPressed == 'd'){
                slash.transform.Rotate(0,0,90);
            }
            if (lastKeyPressed == 'w'){
                slash.transform.Rotate(0,0,-180);
            }
            
    }

    public void hitEnemy(){
        if (lastKeyPressed == 's'){
            rb2D.velocity = new Vector2(rb2D.velocity.x, 20);
        }
    }

    public void bubbleBlast(Vector3 bubblePos){
        Vector3 playerPos = transform.position;

        float blastSpeedX = bubblePos.x - playerPos.x;
        float blastSpeedY = bubblePos.y - playerPos.y;
        
        //Vector2 vector = new Vector2(xSpeed, -blastSpeedY * 15);

        
        float vectorLenght = 20;

        float currentLenght = Mathf.Sqrt((blastSpeedX * blastSpeedX) + (blastSpeedY * blastSpeedY));

        float normalizedX = blastSpeedX/currentLenght;
        float normalizedY = blastSpeedY/currentLenght;

        float newX = -normalizedX * vectorLenght;
        float newY = -normalizedY * vectorLenght * 2;

        xSpeed = Mathf.Round(newX);
        rb2D.velocity = new Vector2(newX, newY);

    }

    void OnCollisionEnter2D(Collision2D coll){
        if (coll.collider.tag == "Enemy"){
            isHit = true;
            knockbackTimeCounter = knockbackTime;
            var direction = transform.InverseTransformPoint (coll.transform.position); 
            
            if (direction.x > 0){
                rb2D.velocity = new Vector2 (-3 , rb2D.velocity.y);
            }
            if (direction.x < 0){
                rb2D.velocity = new Vector2 (3 , rb2D.velocity.y);
            }
        }
    }
}
