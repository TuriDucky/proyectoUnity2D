using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Slash : MonoBehaviour
{
    GameObject player;
    float lifeCounter;
    public float playerDirection;

    void Start()
    {
        player = GameObject.Find("Player");
        lifeCounter = 0.3f;
        
    }

    void FixedUpdate()
    {
        ajustarPosicion();
        lifeCounter -= Time.deltaTime;
        if (lifeCounter <= 0){
            Destroy(gameObject);
        }
        
    }

    void OnTriggerEnter2D(Collider2D collider2D){
        if (collider2D.tag == "Bubble"){
            collider2D.transform.GetComponent<Bubble>().bubbleBlast(collider2D.transform.position);
            Destroy(gameObject);
        }
        if (collider2D.tag == "DashBubble"){
            collider2D.transform.GetComponent<DashBubble>().bubbleBlast(collider2D.transform.position);
            Destroy(gameObject);
        }
        if (collider2D.tag == "Enemy"){
            transform.parent.GetComponent<Player>().hitEnemy();
        }
        
        
    }

    void ajustarPosicion(){
        Vector2 posicion = player.transform.position;
        
        if (playerDirection == 1){
            posicion.y -= 2;
            transform.position = posicion;
        }
        else if(playerDirection == 2){
            posicion.y += 2;
            transform.position = posicion;
        }
        else if(playerDirection == 3){
            posicion.x -= 2;
            transform.position = posicion;
        }
        else{
            posicion.x += 2;
            transform.position = posicion;
        }

    }
}
