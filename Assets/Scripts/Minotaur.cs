using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Minotaur : MonoBehaviour
{
    public int xSpeed;

    public bool isMoving;
    
    public bool isPlayerRight;
    public bool bigAttack;

    public float contadorAtaque;
    public float bigAttackTime;

    Rigidbody2D rb2D;
    SpriteRenderer sr;
    CapsuleCollider2D cc2D;
    Animator animator;
    
    void Start()
    {   
        bigAttackTime = 1.2f;
        rb2D = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cc2D = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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

        if (MinotaurPlayerDetection.hasDetecedPlayer){
            isMoving = false;
            bigAttack = true;
        }
        else{
            isMoving = true;
        }

        if (isMoving){
            animator.SetBool("isRunning", true);
        }
        else{
            animator.SetBool("isRunning", false);
        }

        if (bigAttack){
            if (contadorAtaque <= 0){
                contadorAtaque = bigAttackTime;
            }
            isMoving = false;
            BigAttack();
        }
        
        if (contadorAtaque > 0){
            contadorAtaque -= Time.deltaTime;
            isMoving = false;
        }
        else{
            isMoving = true;
        }
    }

    void FixedUpdate(){
        if (isMoving){
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
    }

    void BigAttack(){
        animator.SetBool("bigAttack", true);
        bigAttack = false;
    }

    private void OnCollisionEnter2D(Collision2D collision2D){
        
    }
}
