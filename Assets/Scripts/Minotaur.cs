using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Minotaur : MonoBehaviour
{
    public int xSpeed;

    public bool isMoving;
    
    public bool isPlayerRight;
    public bool bigAttack;
    public bool spinAttack;
    public bool spinDash;
    public bool isStunned;
    public bool isVulnerable;
    public int wallDirection;
    public int lives;

    public float bigAttackTime;
    public float bigAttackTimeValue;

    public float spinAttackTime;
    public float spinAttackTimeValue;

    public float dashStartDelay;
    public float dashStartDelayValue;

    public float stunTime;
    public float stunTimeValue;
    

    Rigidbody2D rb2D;
    SpriteRenderer sr;
    CapsuleCollider2D cc2D;
    Animator animator;
    
    void Start()
    {   
        spinAttackTime = spinAttackTimeValue;
        wallDirection = Random.Range(0,2);
        
        rb2D = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cc2D = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        checkPlayerPos();
        updateTimers();

        if (!isStunned){
            if (MinotaurPlayerDetection.hasDetecedPlayer && !spinAttack && !spinDash){
                isMoving = false;
                bigAttack = true;
            }

            if (bigAttack  && !spinAttack){
                if (bigAttackTime <= 0){
                    bigAttackTime = bigAttackTimeValue;
                }
                isMoving = false;
                BigAttack();
            }
            
        
            if (isMoving && !spinDash){
                animator.SetBool("isRunning", true);
            }
            else{
                animator.SetBool("isRunning", false);
            }
        }
        

        
    }

    void FixedUpdate(){
        if (!isStunned){
            if (isMoving && !spinAttack && !spinDash){
                if (isPlayerRight){
                    rb2D.velocity = new Vector2(xSpeed, rb2D.velocity.y);
                }
                else{
                    rb2D.velocity = new Vector2(-xSpeed, rb2D.velocity.y);
                }
            }
            else{
                rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            }

            if(spinAttack && !spinDash){
                if (wallDirection == 0){
                    rb2D.velocity = new Vector2(xSpeed, rb2D.velocity.y);
                }
                else{
                    rb2D.velocity = new Vector2(-xSpeed, rb2D.velocity.y);
                }
            }

            if (spinDash && dashStartDelay <= 0){
                if (wallDirection == 0){
                    rb2D.velocity = new Vector2(-10, rb2D.velocity.y);
                }
                else{
                    rb2D.velocity = new Vector2(10, rb2D.velocity.y);
                }
            }
        }
    }

    void BigAttack(){
        animator.SetBool("bigAttack", true);
        if (isPlayerRight){
            sr.flipX = false;
        }
        else{
            sr.flipX = true;
        }
    }

    void checkPlayerPos(){
        if(!bigAttack && !spinAttack){
            if (transform.position.x > GameObject.Find("Player").GetComponent<Transform>().position.x){
                isPlayerRight = false;
                sr.flipX = true;
                cc2D.offset = new Vector2 (0.5f, cc2D.offset.y);
            }
            else{
                isPlayerRight = true;
                sr.flipX = false;
                cc2D.offset = new Vector2 (-0.5f, cc2D.offset.y);
            }
        }
    }

    void updateTimers(){
        if(!isStunned){
            if (dashStartDelay > 0){
                dashStartDelay -=Time.deltaTime;
            }
            else{
                dashStartDelay = 0;
            }

            if (bigAttackTime > 0){
                bigAttackTime -= Time.deltaTime;
                isMoving = false;
            }
            if (bigAttackTime < 0){
                animator.SetBool("bigAttack", false);
                bigAttack = false;
                isMoving = true;
            }

            if (!spinDash && !isStunned){
                if (spinAttackTime > 0 && !spinAttack){
                    spinAttackTime -= Time.deltaTime;
                }
                else{
                    if (!bigAttack){
                        spinAttack = true;
                        spinAttackTime = spinAttackTimeValue;
                    }
                }
            }
            
        }
        else{
            if (stunTime > 0){
                stunTime -= Time.deltaTime;
            }
            else{
                isStunned = false;
                isVulnerable = false;
                animator.SetBool("isStunned", false);
            }   
        }  
    }

    public void hurt(){
        isVulnerable = false;
        lives --;
        if (lives <= 0){
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D){
        if (collision2D.collider.tag == "Wall"){
            if (spinDash){
                spinDash = false;
                animator.SetBool("SpinAttack", false);
                isStunned = true;
                isVulnerable = true;
                animator.SetBool("isStunned", true);
                stunTime = stunTimeValue;
                rb2D.velocity = new Vector2(2, 5);
            }
            if (spinAttack){
                checkPlayerPos();
                spinAttack = false;
                spinDash = true;
                animator.SetBool("SpinAttack", true);
                animator.SetBool("bigAttack", false);
                animator.SetBool("isRunning", false);
                dashStartDelay = dashStartDelayValue; 
            } 
        }

        
    }

    private void OnTriggerEnter2D(Collider2D colider){
        if (colider.tag == "Attack"){
            Debug.Log("fsadf");
            if (isVulnerable){
                hurt();
            }
        }
    }
}
