using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    GameObject player;
    public float vectorLenght;
    
    void Start()
    {
        player = GameObject.Find("Player");
        Debug.Log("hola");
    }

    
    public void bubbleBlast(Vector3 bubblePos){
        
        player.GetComponent<Player>().isDashing = false;
        player.GetComponent<Player>().dashCountCounter = 1;
        Vector3 playerPos = player.transform.position;

        float blastSpeedX = bubblePos.x - playerPos.x;
        float blastSpeedY = bubblePos.y - playerPos.y;

        float currentLenght = Mathf.Sqrt((blastSpeedX * blastSpeedX) + (blastSpeedY * blastSpeedY));

        float normalizedX = blastSpeedX/currentLenght;
        float normalizedY = blastSpeedY/currentLenght;

        float newX = normalizedX * vectorLenght;
        float newY = normalizedY * vectorLenght;

        newX = -newX;
        newY = -newY;
        

        if (newX < 2 && newX > -2){
            newX = 0;
        }
        if (newY < 2 && newY > -2){
            newY = 0;
        }
        
        player.GetComponent<Player>().xSpeed = Mathf.Round(newX);
        player.GetComponent<Player>().rb2D.velocity = new Vector2(newX, newY);

    }
}
