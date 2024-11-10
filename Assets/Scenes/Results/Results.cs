using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class Results : MonoBehaviour
{
    UIDocument menu;

    public AudioSource pointSFX;
    public AudioSource rankSFX;
    public AudioSource music;

    VisualElement results;
    VisualElement record;
    VisualElement rank;
    VisualElement background;
    VisualElement transition;

    Label time;
    Label points;
    Label bonus;
    Label timeBonus;
    Label total;

    Label pointsLabel;
    Label bonusLabel;
    Label timeLabel;
    Label totalLabel;

    public Texture2D S;
    public Texture2D A;
    public Texture2D B;
    public Texture2D C;
    public Texture2D D;

    
    int levelTotal;

    public float Counter; // Temporizador para el valor de abajo
    public float Value; //Tiempo entre diferentes partes de la pantalla de resultados

    bool isGonnaPause;
    int cutscenePoint = -1;
    int numbers = 6;


    float endCounter;
    float endValue = 1;
    bool isEnding;

    public static bool isNewRecord;
    public static int levelScore;
    public static int levelBonus;
    public static int levelTimeBonus;
    public static float levelTime;
    public static int levelRank;

    bool pauseEnded = false;
    void Start()
    {

        levelTimeBonus = calculateTimeBonus();

        levelTotal = levelScore + levelBonus + levelTimeBonus;
        
        menu = GetComponent<UIDocument>();
        VisualElement root = menu.rootVisualElement;

        results = root.Q<VisualElement>("results");
        record = root.Q<VisualElement>("record");
        rank = root.Q<VisualElement>("rank");
        background = root.Q<VisualElement>("background");
        transition = root.Q<VisualElement>("transition");

        time = root.Q<Label>("time");

        points = root.Q<Label>("points");
        bonus = root.Q<Label>("bonus");
        timeBonus = root.Q<Label>("time_bonus");

        pointsLabel = root.Q<Label>("points_label");
        bonusLabel = root.Q<Label>("bonus_label");
        timeLabel = root.Q<Label>("time_label");
        totalLabel = root.Q<Label>("total_label");

        total = root.Q<Label>("total");
        Counter = Value;
        
        isGonnaPause = true;

        
    }

    bool timers = true;
    int rotation;
    void Update(){
        rotation ++;
        
        background.transform.rotation *= Quaternion.Euler(0f, 0f, 5 * Time.deltaTime);

        if (Input.anyKeyDown)
        {
            if (cutscenePoint >= 7)
            {
                transitionEnd();
            }
        }
        


        if(isEnding){
            if (music.volume > 0)
            {
                music.volume -= Time.deltaTime / 1.5f;
            }
            else
            {
                music.volume = 0;
            }
            endCounter -= Time.deltaTime;
            if (endCounter <= 0){
                SceneManager.LoadSceneAsync("Main Menu");
            }
        }
        
        if(isGonnaPause){
            pauseEnded = pause();
        }

        if (pauseEnded){
            switch(cutscenePoint){
                case -1:
                    transitionStart();
                    isGonnaPause = true;
                    Counter = Value;
                    cutscenePoint ++;
                    break;
                case 0:
                    results.RemoveFromClassList("results_start");
                    isGonnaPause = true;
                    Counter = Value;
                    cutscenePoint ++;
                    break;
                case 1:
                    if (timers){
                        Counter = 0.2f;
                        timers = false;
                    }
                    
                    isGonnaPause = false;
                    time.text = formatTimeToString(generateText(time, numbers, formatTime(levelTime)));
                    break;
                case 2:
                    Debug.Log(isNewRecord);
                    if(isNewRecord){
                        Debug.Log("New Record");
                        record.RemoveFromClassList("record_start");
                        isGonnaPause = true;
                        Counter = Value;
                        isNewRecord = false;        
                    }
                    cutscenePoint ++;
                    break;
                case 3:
                    pointsLabel.text = "Points:";
                    if (timers){
                        Counter = 0.2f;
                        timers = false;
                    }

                    isGonnaPause = false;
                    points.text = generateText(points, numbers, levelScore);
                    break;
                case 4:
                    bonusLabel.text = "Bonus:";
                    if (timers){
                        Counter = 0.2f;
                        timers = false;
                    }
                    
                    isGonnaPause = false;
                    bonus.text = generateText(bonus, numbers, levelBonus);
                    break;
                case 5:
                    timeLabel.text = "Time Bonus:";
                    if (timers){
                        Counter = 0.2f;
                        timers = false;
                    }
                    
                    isGonnaPause = false;
                    timeBonus.text = generateText(timeBonus, numbers, levelTimeBonus);
                    break;
                case 6:
                    totalLabel.text = "Total:";
                    if (timers){
                        Counter = 0.2f;
                        timers = false;
                    }
                    
                    isGonnaPause = false;
                    total.text = generateText(total, numbers, levelTotal);
                    break;
                case 7:
                    rankSFX.Play();
                    setRankImage();
                    rank.RemoveFromClassList("rank_start");
                    isGonnaPause = true;
                    Counter = Value;
                    cutscenePoint ++;
                    break;
            }
        }
    }

    private void transitionStart(){
        transition.RemoveFromClassList("transition_start");
    }

    private void transitionEnd(){
        transition.AddToClassList("transition_start");
        isEnding = true;
        endCounter = endValue;
    }

    private void setRankImage(){
        switch(levelRank){
            case 4:
                rank.style.backgroundImage = Background.FromTexture2D(S);
                break;
            case 3:
                rank.style.backgroundImage = Background.FromTexture2D(A);
                break;
            case 2:
                rank.style.backgroundImage = Background.FromTexture2D(B);
                break;
            case 1:
                rank.style.backgroundImage = Background.FromTexture2D(C);
                break;
            case 0:
                rank.style.backgroundImage = Background.FromTexture2D(D);
                break;
        }
    }

    private string formatTimeToString(string time){
        return $"{time.Substring(0, 2)}:{time.Substring(2, 2)}:{time.Substring(4, 2)}";
    }

    private int formatTime(float time){
        int minutos = (int) (time / 60f);
        int segundos = (int) (time - minutos * 60f);
        int decimas = (int) ((time - (int)time) * 100f);

        string timeText = String.Format("{0:00}{1:00}{2:00}", minutos, segundos, decimas);
       
        return Convert.ToInt32(timeText);
    }
    

    private string generateText(Label label, int theNumbers, int points){
        pointSFX.Play();
        String cadena = generateNumbers(theNumbers, points.ToString());
        Counter -= Time.deltaTime;
        if (Counter <= 0){
            numbers --;
            Counter = 0.2f;
            if (numbers < 0){
                pointSFX.Stop();
                numbers = 6;
                isGonnaPause = true;
                Counter = Value;
                cutscenePoint ++;
            }
        }

        return cadena;
    }

    bool pause(){
        
        Counter -= Time.deltaTime;
        
        if (Counter > 0){
            return false;
        }
        else{
            Debug.Log("end pause");
            Counter = 0;
            return true;
        }
    }


    private string generateNumbers(int numbers, string score){
        string result = "";

        if (score.Length < 6){
            string ogScore = score;
            string charsToAdd = "";
            for (int rep = score.Length; rep < 6; rep ++){
                charsToAdd += "0";
            }
            charsToAdd = charsToAdd + ogScore;
            score = charsToAdd;
        }
        
        
        for (int rep = 0; rep < numbers; rep ++){
            int randomNum = UnityEngine.Random.Range(0,10);
            result += (randomNum).ToString();

            score = score.Substring(1);
        }


        result += score;

        return result;
    }

    private int calculateTimeBonus(){
        int score = 0;
        int value = 300 - Convert.ToInt32(levelTime);
        if (value > 0){
            score += value * 200;
        }
        return score;
    }
}
