using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class Level : MonoBehaviour
{

    public TMP_Text timerUI;
    float timer;

    int minutos;
    int segundos;
    int decimas;

    public Image moneda1;
    public Image moneda2;
    public Image moneda3;
    public Image moneda4;
    public Image moneda5;

    public Image moneda1Blank;
    public Image moneda2Blank;
    public Image moneda3Blank;
    public Image moneda4Blank;
    public Image moneda5Blank;

    void Start()
    {
        
    }

    void Update()
    {
        timer += Time.deltaTime;

        minutos = (int) (timer / 60f);
        segundos = (int) (timer - minutos * 60f);
        decimas = (int) ((timer - (int)timer) * 100f);

        timerUI.text = String.Format("{0:00}:{1:00}:{2:00}", minutos, segundos, decimas);
    }

    public void colectCoin(int number){
        switch(number){
            case 1:
                moneda1Blank.enabled = false;
                moneda1.enabled = true;
                break;
            case 2:
                moneda2Blank.enabled = false;
                moneda2.enabled = true;
                break;
            case 3:
                moneda3Blank.enabled = false;
                moneda3.enabled = true;
                break;
            case 4:
                moneda4Blank.enabled = false;
                moneda4.enabled = true;
                break;
            case 5:
                moneda5Blank.enabled = false;
                moneda5.enabled = true;
                break;
        }
    }
}
