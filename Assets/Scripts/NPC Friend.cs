using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NPC_Friend : MonoBehaviour
{
    public float xSpeed;
    int direction;
    float counter;
    public float counterValue;
    bool isMoving;
    private Animator andar;

    Rigidbody2D rb2D;
    SpriteRenderer sr;

    void Start()
    {   
        counter = 0;
        rb2D = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        andar = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (counter <= 0){
            counter = counterValue;
            if (isMoving){
                isMoving = false;
            }
            else{
                isMoving = true;
                direction = Random.Range(0,2);
            }
        }
        counter -= Time.deltaTime;
        
        
    }

    void FixedUpdate(){
         if (isMoving){
            if (direction == 1){
                sr.flipX = false;
                rb2D.velocity = new Vector2(xSpeed, rb2D.velocity.y);
                andar.Play("Run");
            }
            if (direction == 0){
                sr.flipX = true;
                rb2D.velocity = new Vector2(-xSpeed, rb2D.velocity.y);
                andar.Play("Run");
            }
        }

        if (!isMoving){
            rb2D.velocity = new Vector2 (0, rb2D.velocity.y);
            andar.Play("Idle");
        }
    }
}
