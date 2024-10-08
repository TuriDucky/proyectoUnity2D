using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StopCamara : MonoBehaviour
{
    public bool blockY;
    public bool blockX;
    void Start()
    {
        GameObject.Find("Virtual Camera").GetComponent<LockCameraY>().startFollowing();
    }
    void OnTriggerEnter2D(Collider2D collider2D){
        if (collider2D.tag == "Player"){
            if (blockY){
                GameObject.Find("Virtual Camera").GetComponent<LockCameraY>().setY();
            }
            if (blockX){
                GameObject.Find("Virtual Camera").GetComponent<LockCameraY>().setX();
            }
            
        }
        
    }
    void OnTriggerExit2D(Collider2D collider2D){
        if (collider2D.tag == "Player"){
            GameObject.Find("Virtual Camera").GetComponent<LockCameraY>().startFollowing();
        }
    }
}
