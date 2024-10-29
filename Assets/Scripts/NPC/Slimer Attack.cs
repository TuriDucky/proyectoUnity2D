using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimerAttack : MonoBehaviour
{
    bool detectedPlayer;
    void OnTriggerEnter2D(Collider2D collider2D){
        if (collider2D.tag == "Player"){
            detectedPlayer = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider2D){
        if (collider2D.tag == "Player"){
            detectedPlayer = false;
        }
    }

    public bool getPlayer(){
        return detectedPlayer;
    }
}
