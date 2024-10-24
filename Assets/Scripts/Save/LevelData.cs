using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    private string levelName;
    private bool isBeaten;
    private float bestTime;
    private int maxScore;
    private int rank; //-1 = ?, 0 = D, 1 = C, 2 = B, 3 = A, 4 = S
    private bool item1;
    private bool item2 ;
    private bool item3;
    private bool item4;
    private bool item5;

    

    public LevelData(string name, bool beaten, float time, bool coll1, bool coll2, bool coll3, bool coll4, bool coll5, int score, int newRank){
        setName(name);
        setbeaten(beaten);
        setBestTime(time);
        setScore(score);
        setRank(newRank);

        setItem1(coll1);
        setItem2(coll2);
        setItem3(coll3);
        setItem4(coll4);
        setItem5(coll5);

        
    }

    public string getName(){
        return levelName;
    }

    public void setName(String newName){
        this.levelName = newName;
    }

    public bool getBeaten(){
        return isBeaten;
    }

    public void setbeaten(bool beaten){
        this.isBeaten = beaten;
    }

    public float getBestTime(){
        return bestTime;
    }

    public void setBestTime(float time){
        this.bestTime = time;
    }

    public int getScore(){
        return maxScore;
    }

    public void setScore(int newScore){
        this.maxScore = newScore;
    }

    public int getRank(){
        return rank;
    }

    public void setRank(int newRank){
        this.rank = newRank;
    }

    public bool getItem1(){
        return item1;
    }

    public void setItem1(bool item){
        this.item1 = item;
    }

    public bool getItem2(){
        return item2;
    }

    public void setItem2(bool item){
        this.item2 = item;
    }

    public bool getItem3(){
        return item3;
    }

    public void setItem3(bool item){
        this.item3 = item;
    }

    public bool getItem4(){
        return item4;
    }

    public void setItem4(bool item){
        this.item4 = item;
    }

    public bool getItem5(){
        return item5;
    }

    public void setItem5(bool item){
        this.item5 = item;
    }
}
