using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class edgeDetection : MonoBehaviour
{
    private bool edge;
    void OnTriggerExit2D(Collider2D collider2D){
        
        if (collider2D.tag == "terrain"){
            edge = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collider2D){
        if (collider2D.tag == "terrain"){
            edge = false;
        }
    }

    public bool getEdge(){
        return edge;
    }
    public void setEdge(bool theEdge){
        edge = theEdge;
    }
}
