using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public GameObject player;
    float lifeCounter;
    void Start()
    {
        player = GameObject.Find("Player");
        lifeCounter = 10;
    }

    void FixedUpdate()
    {
        transform.position = player.transform.position;
        lifeCounter --;
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
            Enemy enemigo =  collider2D.gameObject.GetComponent<Enemy>();
            transform.parent.GetComponent<Player>().hitEnemy(enemigo.isDying);
            enemigo.Death();
        }
        if (collider2D.tag == "Breakable"){
            Destroy(collider2D.gameObject);
            Destroy(gameObject);
        }
        
    }
}
