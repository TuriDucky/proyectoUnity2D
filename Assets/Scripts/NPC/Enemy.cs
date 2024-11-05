using UnityEngine;

public class Enemy : MonoBehaviour
{

    public bool isDying;
    public int pointValue;
    public int lives = 1;
    
    void Start()
    {   
        isDying = false;
    }

    public void setDying(bool die){
        isDying = die;
    }

    public bool getDying(){
        return isDying;
    }

    public void setPoints(int points){
        this.pointValue = points;
    }

    public int getPoints(){
        return pointValue;
    }

    public void setLives(int lp){
        lives = lp;
    }

    public int getLives(){
        return lives;
    }
}
