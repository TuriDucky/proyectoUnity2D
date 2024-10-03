using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int direction;
    public float counter;
    public float counterValue;
    public bool isMoving;
    public float deathCounter;
    public bool isDying;

    Rigidbody2D rb2D;
    BoxCollider2D bc2D;
    void Start()
    {   
        isDying = false;
        counter = 0;
        rb2D = GetComponent<Rigidbody2D>();
        bc2D = GetComponent<BoxCollider2D>();
        deathCounter = 200;
    }

    // Update is called once per frame
    void Update()
    {
        if (counter == 0){
            counter = counterValue;
            if (isMoving){
                isMoving = false;
            }
            else{
                isMoving = true;
                direction = Random.Range(0,2);
            }
        }
        counter --;
        
        if (isDying){
            deathCounter --;
            if (deathCounter == 0){
                Destroy(gameObject);
            }
        }
    }

    void FixedUpdate(){
        if (isMoving){
            if (direction == 1){
                rb2D.velocity = new Vector2(3, rb2D.velocity.y);
            }
            if (direction == 0){
                rb2D.velocity = new Vector2(-3, rb2D.velocity.y);
            }
        }

        if (!isMoving){
            rb2D.velocity = new Vector2 (0, rb2D.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D){
        if (collision2D.collider.tag == "Wall"){
            if (direction == 0){
                direction = 1;
            }
            if (direction == 1){
                direction = 0;
            }
        }

        if (collision2D.collider.tag == "Attack"){
            Destroy(gameObject);
        }
    }

    public void Death(){
        rb2D.velocity = new Vector2(rb2D.velocity.x, 20);
        bc2D.isTrigger = true;
        isDying = true;
    }
}
