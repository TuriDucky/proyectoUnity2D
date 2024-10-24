using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGoal : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider2D){
        if (collider2D.tag == "Player"){
            GameObject.Find("Level").GetComponent<Level>().endLevel(); 
        }
    }
}

