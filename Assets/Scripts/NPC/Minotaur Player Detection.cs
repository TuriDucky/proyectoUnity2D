using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurPlayerDetection : MonoBehaviour
{   
    static public bool hasDetecedPlayer; 

    void OnTriggerEnter2D(Collider2D collider2D){
        if (collider2D.tag == "Player"){
            hasDetecedPlayer = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider2D){
        if (collider2D.tag == "Player"){
            hasDetecedPlayer = false;
        }
    }
}
