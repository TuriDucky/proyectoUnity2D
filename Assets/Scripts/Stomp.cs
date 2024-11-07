using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stomp : MonoBehaviour
{
    GameObject player;
    float lifeCounter;

 
    void Start()
    {
        player = GameObject.Find("Player");

        lifeCounter = 99999999;

    }

    void FixedUpdate()
    {
        ajustarPosicion();
        lifeCounter --;
        if (lifeCounter <= 0){
            Destroy(gameObject);
        }
        
    }

    void OnTriggerEnter2D(Collider2D collider2D){
        if (collider2D.tag == "Bubble"){
            player.GetComponent<Player>().setStomp(false);
            collider2D.transform.GetComponent<Bubble>().bubbleBlast(collider2D.transform.position);
            Destroy(gameObject);
        }
        if (collider2D.tag == "DashBubble"){
            player.GetComponent<Player>().setStomp(false);
            collider2D.transform.GetComponent<DashBubble>().bubbleBlast(collider2D.transform.position);
            Destroy(gameObject);
        }
        if (collider2D.tag == "Enemy"){
            transform.parent.GetComponent<Player>().hitEnemy();
        }
        if (GroundCheck.isGrounded){
            Destroy(gameObject);
        }
        if (collider2D.tag == "Semisolid"){
            Destroy(gameObject);
        }
    
    }

    void ajustarPosicion(){
        Vector2 posicion = player.transform.position;
    
        posicion.y -= 1;
        posicion.x += 0.2f;
        transform.position = posicion;
    }
}
