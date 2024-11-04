using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using Image = UnityEngine.UI.Image;

public class Level : MonoBehaviour
{
    LevelData level;

    public TMP_Text timerUI;
    public TMP_Text pointsUI;
    float timer;
    int displayScore;
    int coinScore;
    int score;
    
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
        addScore(0);
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
        addDisplayScore(10000);
        coinScore += 10000;
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
        
        if (level.getBestTime() == 0|| level.getBestTime() > timer){
            Results.isNewRecord = true;
            level.setBestTime(timer);
        }

        if (level.getScore() < score){
            
            level.setScore(score);  
        }
        
        setRank();
        GameData.saveLevelData(level);
        setResultsValues();
        SceneManager.LoadSceneAsync("Results");
        
    }

    public void addScore(int points){
        score += points;
        displayScore += points;
        pointsUI.text = displayScore.ToString() + " pts";
    }

    public void addDisplayScore(int points){
        displayScore += points;
        pointsUI.text = displayScore.ToString() + " pts";
    }

    public void setResultsValues(){
        Results.levelTime = timer;
        Results.levelScore = score;
        Results.levelBonus = coinScore;
    }

    public int timeBonus(){
        int timeBonus = 0;
        int value = 300 - Convert.ToInt32(timer);
        if (value > 0){
            timeBonus += value * 200;
        }
        return timeBonus;
    }

    public void setRank(){
        int totalScore = displayScore + timeBonus();
        Debug.Log(totalScore);
        if (totalScore >= 100000){
            Debug.Log("Rank S");
            level.setRank(4);
        }
        else if (totalScore >= 90000){
            Debug.Log("Rank A");
            level.setRank(3);
        }
        else if (totalScore >= 70000){
            Debug.Log("Rank B");
            level.setRank(2);
        }
        else if (totalScore >= 50000){
            Debug.Log("Rank C");
            level.setRank(1);
        }
        else{
            Debug.Log("Rank D");
            level.setRank(0);
        }
        Results.levelRank = level.getRank();
    }
}
