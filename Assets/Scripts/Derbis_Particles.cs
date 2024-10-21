using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Derbis_Particles : MonoBehaviour
{
    Rigidbody2D rb2D;
    float timer;
    public float lockX; // 0 = fuerza aleatoria, 1 = fuerza a la derecha, 2 = fuerza a la izquierda, 3 = no hay fuerza horizontal
    public float maxForce;
    void Start()
    {
        timer = 5;
        rb2D = GetComponent<Rigidbody2D>();

        float vectorx = 0;
        float vectory = 5;
        switch (lockX){
            case 0:
                vectorx = Random.Range(-maxForce , maxForce);
                break;
            case 1:
                vectorx = Random.Range(0,maxForce);
                break;
            case 2:
                vectorx = Random.Range(-maxForce, 0);
                break;
        }
        rb2D.velocity = new Vector2(vectorx, vectory);
        rb2D.angularVelocity = Random.Range(-500,500);
    }

    void Update(){
        timer -= Time.deltaTime;
        if (timer <= 0){
            Destroy(gameObject);
        }
    }

    
}
