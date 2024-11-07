using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool singleUse;
    void OnTriggerEnter2D(Collider2D collider2D){
        if(collider2D.tag == "Player"){
            collider2D.transform.GetComponent<Player>().setCheckpoint(transform.position);
            if (singleUse){
                Destroy(gameObject);
            }
        }
    }
}
