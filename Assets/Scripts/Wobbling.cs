using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobbling : MonoBehaviour
{
    public float maxSpeed; // Items = 1
    public float acceleration; // Items = 0.005
    bool goingUp;
    public float force; // Items = 1
    public float ySpeed;
    Rigidbody2D rb2D;
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        goingUp = true;
    }

    // Update is called once per frame
    void Update()
    {

        if(ySpeed > maxSpeed){
            goingUp = false;
        }
        if(ySpeed < -maxSpeed){
            goingUp = true;
        }

        if (goingUp){
            ySpeed = ySpeed + force * acceleration * Time.deltaTime;
        }
        else{
            ySpeed = ySpeed - force * acceleration * Time.deltaTime;
        }

        rb2D.velocity = new Vector2(rb2D.velocity.x, ySpeed);
    }
}
