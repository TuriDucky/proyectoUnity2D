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
                moneda1.enabled = false;
                break;

        }
    }
}
