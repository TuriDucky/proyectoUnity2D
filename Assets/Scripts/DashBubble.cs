using System;
using UnityEngine;

public class DashBubble : MonoBehaviour
{
    
    GameObject player;
    public float vectorLenght;
    public bool lockAxis;
    
    void Start()
    {
        player = GameObject.Find("Player");
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

        if (!lockAxis){
            if (newX < 5 && newX > -5){
                newX = 0;
            }
            if (newY < 5 && newY > -5){
                newY = 0;
            }
        }
        else{

            float distanciaMinusVectorLenghtX = Math.Abs(newX - (-vectorLenght));
            float distancia0X = Math.Abs(newX - 0);
            float distanciavectorLenghtX = Math.Abs(newX - vectorLenght);

            if (distanciaMinusVectorLenghtX <= distancia0X && distanciaMinusVectorLenghtX <= distanciavectorLenghtX){
                newX = -vectorLenght;
            }
            else if (distancia0X <= distanciaMinusVectorLenghtX && distancia0X <= distanciavectorLenghtX){
                newX = 0;
            }
            else{
                newX = vectorLenght;
            }

            float distanciaMinusVectorLenghtY = Math.Abs(newY - (-vectorLenght));
            float distancia0Y = Math.Abs(newY - 0);
            float distanciaVectorLenghtY = Math.Abs(newY - vectorLenght);

            if (distanciaMinusVectorLenghtY <= distancia0Y && distanciaMinusVectorLenghtY <= distanciaVectorLenghtY){
                newY = -vectorLenght;
            }
            else if (distancia0Y <= distanciaMinusVectorLenghtY && distancia0Y <= distanciaVectorLenghtY){
                newY = 0;
            }
            else{
                newY = vectorLenght;
            }
        }
        
        player.GetComponent<Player>().xSpeed = Mathf.Round(newX);
        player.GetComponent<Player>().rb2D.velocity = new Vector2(newX, newY);

    }
}
