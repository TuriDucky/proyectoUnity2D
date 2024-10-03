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
            transform.parent.GetComponent<Player>().bubbleBlast(collider2D.transform.position);
        }
        if (collider2D.tag == "Enemy"){
            collider2D.gameObject.GetComponent<Enemy>().Death();
            transform.parent.GetComponent<Player>().hitEnemy();
        }
        
    }
}
