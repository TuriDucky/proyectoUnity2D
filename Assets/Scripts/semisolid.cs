using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class semisolid : MonoBehaviour
{
    PlatformEffector2D effector2D;
    public float contador;
    void Start()
    {
        effector2D = GetComponent<PlatformEffector2D>();
        contador = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space)){
            effector2D.rotationalOffset = 180f;
            contador = 0.2f;
        }

        if (contador > 0.0f){
            contador -= Time.deltaTime;
        }
        if (contador <= 0.0f){
            contador = 0;
            effector2D.rotationalOffset = 0f;
        }
        

        
    }
}
