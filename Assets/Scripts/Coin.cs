using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int numero;
    void OnTriggerEnter2D(Collider2D collider2D){
        if (collider2D.tag == "Player"){
            GameObject.Find("Level").GetComponent<Level>().colectCoin(numero);
            Destroy(gameObject);
        }
    }
}
