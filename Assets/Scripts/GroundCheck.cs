using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    static public bool isGrounded;

    private void OnTriggerEnter2D(Collider2D coll){
        if(coll.tag == "terrain"){
            isGrounded = true;
        }
        
    }

    private void OnTriggerStay2D(Collider2D coll){
        if(coll.tag == "terrain"){
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D coll){
        if(coll.tag == "terrain"){
            isGrounded = false;
        }
        
    }
}
