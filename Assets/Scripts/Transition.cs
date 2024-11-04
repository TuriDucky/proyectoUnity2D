using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    RectTransform pos;
    

    public float currentPosX;
    public float currentPosY;
    public float xSpeed;
    public float ySpeed;
    public bool isOpening;
    public bool isClosing;
    public bool sceneTransition;


    void Start()
    {
        pos = GetComponent<RectTransform>();

        pos.sizeDelta = new Vector2(pos.sizeDelta.x, pos.sizeDelta.y);
        
        
        currentPosX = 0;
        currentPosY = 0;
        pos.anchoredPosition = new Vector3(currentPosX, currentPosY, pos.transform.position.z);
        isOpening = true;

    }

    void Update()
    {
        if (isOpening){
            if (currentPosX > -1000){
                
                pos.anchoredPosition = new Vector3(currentPosX - xSpeed * Time.deltaTime, currentPosY, pos.transform.position.z);
                currentPosX = currentPosX - xSpeed * Time.deltaTime;


            }
            else{
                isOpening = false;
                
            }
        }

        if (isClosing){
            if (currentPosX < 0){

                pos.anchoredPosition = new Vector3(currentPosX + xSpeed * Time.deltaTime, currentPosY, pos.transform.position.z);
                currentPosX = currentPosX + xSpeed * Time.deltaTime;
            }
            else{
                isClosing = false;
                if (sceneTransition){
                    GameObject.Find("Level").GetComponent<Level>().endLevel();    
                }
            }
        }
    }

    public void open(){
        isOpening = true;
    }

    public void close(bool scene){
        isClosing = true;
        sceneTransition = scene;
    }

    
    
}
