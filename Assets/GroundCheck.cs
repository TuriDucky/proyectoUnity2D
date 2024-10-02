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
            Debug.Log("Entrado");
        }
        
    }

    private void OnTriggerExit2D(Collider2D coll){
        if(coll.tag == "terrain"){
            isGrounded = false;
            Debug.Log("Salido");
        }
        
    }
}
