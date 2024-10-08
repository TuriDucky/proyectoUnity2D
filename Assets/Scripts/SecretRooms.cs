using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SecretRooms : MonoBehaviour
{
    TilemapRenderer tmr;
    Tilemap tilemap;
    Color elColor;


    public bool isInside;
    // Start is called before the first frame update
    void Start()
    {
        tmr = GetComponent<TilemapRenderer>();
        tilemap = GetComponent<Tilemap>();
        elColor.r = 255f;
        elColor.g = 255f;
        elColor.b = 255f;
        elColor.a = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInside && tilemap.color.a > 0){
            elColor.a -= Time.deltaTime * 2;
            tilemap.color = elColor;
        }      
    }

    void OnTriggerEnter2D(Collider2D collider2D){
        
        if (collider2D.tag == "Player"){
            Debug.Log("entrado");
            isInside = true;
        }
    }

    void OnTriggerStay2D(Collider2D collider2D){
        if (collider2D.tag == "Player"){
            isInside = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider2D){
        
        if (collider2D.tag == "Player"){
            Debug.Log("salido");
            isInside = false;
            elColor.a = 1f;
            tilemap.color = elColor;
        }
    }
}
