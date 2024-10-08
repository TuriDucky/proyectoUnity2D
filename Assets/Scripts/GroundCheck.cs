using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    static public bool isGrounded;
    static public bool touchingSemisolid = false;

    private void OnTriggerEnter2D(Collider2D coll){
        if(coll.tag == "terrain" || coll.tag == "Breakable" || coll.tag == "Semisolid"){
            isGrounded = true;
            if(coll.tag == "Semisolid"){
                touchingSemisolid = true;
            }
        }
        
    }

    private void OnTriggerStay2D(Collider2D coll){
        if(coll.tag == "terrain" || coll.tag == "Breakable" || coll.tag == "Semisolid"){
            isGrounded = true;
            if(coll.tag == "Semisolid"){
                touchingSemisolid = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D coll){
        if(coll.tag == "terrain" || coll.tag == "Breakable" || coll.tag == "Semisolid"){
            isGrounded = false;
            if(coll.tag == "Semisolid"){
                touchingSemisolid = false;
            }
        }
        
    }
}
