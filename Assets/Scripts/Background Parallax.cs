using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    float startPos;
    float lenght;
    public GameObject cam;
    public float parallaxEffectX;
    public float parallaxEffectY;
    float yStart;
    void Start()
    {
        startPos = transform.position.x;
        yStart = transform.position.y;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
        
    }
    
    void FixedUpdate()
    {
        
        float distanceX = cam.transform.position.x * parallaxEffectX;
        float distanceY = cam.transform.position.y * -parallaxEffectY / 4;

        transform.position = new Vector3(startPos + distanceX, yStart + distanceY, transform.position.z);
    }
}
