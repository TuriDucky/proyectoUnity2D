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

    public float contadorAtaqueGiratorio;
    public float TiempoAtaqueGiratorio;

    public bool spinAttack;
    public int wallDirection;

    public bool spinDash;

    Rigidbody2D rb2D;
    SpriteRenderer sr;
    CapsuleCollider2D cc2D;
    Animator animator;
    
    void Start()
    {   
        contadorAtaqueGiratorio = TiempoAtaqueGiratorio;
        wallDirection = Random.Range(0,2);
        
        rb2D = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cc2D = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (contadorAtaqueGiratorio > 0 && !spinAttack){
            contadorAtaqueGiratorio -= Time.deltaTime;
        }
        else{
            if (!bigAttack){
                spinAttack = true;
                contadorAtaqueGiratorio = TiempoAtaqueGiratorio;
            }
            
        }

        if (transform.position.x > GameObject.Find("Player").GetComponent<Transform>().position.x && !spinAttack && !spinDash){
            isPlayerRight = false;
            sr.flipX = true;
            cc2D.offset = new Vector2 (0.5f, cc2D.offset.y);
        }
        else{
            isPlayerRight = true;
            sr.flipX = false;
            cc2D.offset = new Vector2 (-0.5f, cc2D.offset.y);
        }

        if (MinotaurPlayerDetection.hasDetecedPlayer && !spinAttack && !spinDash){
            isMoving = false;
            bigAttack = true;
        }

        if (bigAttack  && !spinAttack){
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
        if (contadorAtaque < 0){
            animator.SetBool("bigAttack", false);
            bigAttack = false;
            isMoving = true;
        }

        if (isMoving){
            animator.SetBool("isRunning", true);
        }
        else{
            animator.SetBool("isRunning", false);
        }
    }

    void FixedUpdate(){
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

        if (spinDash){
            if (wallDirection == 0){
                rb2D.velocity = new Vector2(-10, rb2D.velocity.y);
            }
            else{
                rb2D.velocity = new Vector2(10, rb2D.velocity.y);
            }
        }
    }

    void BigAttack(){
        animator.SetBool("bigAttack", true);
    }

    private void OnCollisionEnter2D(Collision2D collision2D){
        if (collision2D.collider.tag == "Wall"){
            if (spinDash){
                spinDash = false;
                animator.SetBool("SpinAttack", false);
            }
            if (spinAttack == true){
                spinAttack = false;
                spinDash = true;
                animator.SetBool("SpinAttack", true);
                animator.SetBool("bigAttack", false);
                animator.SetBool("isRunning", false);
            } 
        }
    }
}
