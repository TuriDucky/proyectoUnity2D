using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;

public class Level : MonoBehaviour
{
    LevelData level;

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
        level = GameData.getTutorial();
        loadlevel();
    }

    private void loadlevel(){
        if (level.getItem1()){
            moneda1Blank.enabled = false;
            moneda1.enabled = true;
        }
        if (level.getItem2()){
            moneda2Blank.enabled = false;
            moneda2.enabled = true;
        }
        if (level.getItem3()){
            moneda3Blank.enabled = false;
            moneda3.enabled = true;
        }
        if (level.getItem4()){
            moneda4Blank.enabled = false;
            moneda4.enabled = true;
        }
        if (level.getItem5()){
            moneda5Blank.enabled = false;
            moneda5.enabled = true;
        }
        
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
                level.setItem1(true);
                moneda1Blank.enabled = false;
                moneda1.enabled = true;
                break;
            case 2:
                level.setItem2(true);
                moneda2Blank.enabled = false;
                moneda2.enabled = true;
                break;
            case 3:
                level.setItem3(true);
                moneda3Blank.enabled = false;
                moneda3.enabled = true;
                break;
            case 4:
                level.setItem4(true);
                moneda4Blank.enabled = false;
                moneda4.enabled = true;
                break;
            case 5:
                level.setItem5(true);
                moneda5Blank.enabled = false;
                moneda5.enabled = true;
                break;
        }
    }

    public void endLevel(){
        Debug.Log("Finish");
        level.setbeaten(true);
        level.setBestTime(timer);
        GameData.saveLevelData(level);
        SceneManager.LoadSceneAsync("Main Menu");
        
    }
}
