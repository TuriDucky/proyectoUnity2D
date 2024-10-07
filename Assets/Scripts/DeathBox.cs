using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider2D){
        if (collider2D.tag == "Player"){
            Debug.Log("Muerto");
            collider2D.transform.GetComponent<Player>().Respawn();
        }
    }
}
