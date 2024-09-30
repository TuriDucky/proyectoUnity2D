using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    static public BoxCollider2D bc2D;
    static public bool grounded;
    void Start()
    {
        bc2D = GetComponent<BoxCollider2D>();
        grounded = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(BoxCollider2D bc2D){
        grounded = true;
    }

    void OnCollisionExit2D(BoxCollider2D bc2D){
        grounded = false;
    }
}
