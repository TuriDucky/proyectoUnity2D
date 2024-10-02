using System.Collections;
using System.Collections.Generic;
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
}
