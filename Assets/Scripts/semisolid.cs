using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class semisolid : MonoBehaviour
{
    PlatformEffector2D effector2D;
    float contador;
    void Start()
    {
        effector2D = GetComponent<PlatformEffector2D>();
        contador = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space)){
            effector2D.rotationalOffset = 180f;
            contador = 50;
        }

        if (contador > 0){
            contador --;
        }
        if (contador == 0){
            effector2D.rotationalOffset = 0f;
        }
        

        
    }
}
