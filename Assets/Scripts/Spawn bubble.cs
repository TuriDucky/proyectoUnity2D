using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawnbubble : MonoBehaviour
{
    public GameObject bubble;
    public GameObject dashBubble;
    public int trigger;
    void OnTriggerEnter2D(Collider2D collider2D){
        if (collider2D.tag == "Player"){
            if (trigger == 1){
                GameObject ojebt = Instantiate(bubble, new Vector3(150, 10, 0), Quaternion.identity);
                GameObject ojebt2 = Instantiate(bubble, new Vector3(170, 18, 0), Quaternion.identity);
            }
            else if (trigger == 2){
                GameObject ojebt = Instantiate(dashBubble, new Vector3(191, 25, 0), Quaternion.identity);
                ojebt.GetComponent<DashBubble>().setVector(60);
                
            }
            
            Destroy(gameObject);
            
        }
        
    }
}
