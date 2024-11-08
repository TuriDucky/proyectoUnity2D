using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{
    UIDocument menu;
    int page;
    int numPages = 2;

    public AudioSource playSFX;
    public AudioSource arrowSFX;
    public AudioSource music;
    
    Button play;
    Button back;
    Button next;
    Label levelName;
    Label score;
    Label time;

    VisualElement item1;
    VisualElement item2;
    VisualElement item3;
    VisualElement item4;
    VisualElement item5;
    VisualElement image;
    VisualElement trophy;
    VisualElement transition;
    VisualElement rank;
    VisualElement background;

    public Texture2D missing;
    public Texture2D apple;
    public Texture2D ballon;
    public Texture2D canvas;
    public Texture2D train;
    public Texture2D fly;
    public Texture2D S;
    public Texture2D A;
    public Texture2D B;
    public Texture2D C;
    public Texture2D D;

    public Texture2D gold;
    public Texture2D silver;
    public Texture2D bronze;
    public Texture2D tutorial;
    public Texture2D level1;

    public float trasitionTimeCounter;
    public float trasitionTimeValue;

    void OnEnable(){
        page = 1;
        menu = GetComponent<UIDocument>();
        VisualElement root = menu.rootVisualElement;

        play = root.Q<Button>("play");
        back = root.Q<Button>("back"); 
        next = root.Q<Button>("next");

        levelName = root.Q<Label>("levelName");
        score = root.Q<Label>("score"); 
        time = root.Q<Label>("time");

        rank = root.Q<VisualElement>("rank");
        item1 = root.Q<VisualElement>("item1");
        item2 = root.Q<VisualElement>("item2");
        item3 = root.Q<VisualElement>("item3");
        item4 = root.Q<VisualElement>("item4");
        item5 = root.Q<VisualElement>("item5");

        image = root.Q<VisualElement>("image");
        trophy = root.Q<VisualElement>("trophy");
        transition = root.Q<VisualElement>("transition");
        background = root.Q<VisualElement>("background");

        
        //transitionStart();
        Invoke("transitionStart", 0.1f);

        back.SetEnabled(false);
        if (page >= numPages){
            next.SetEnabled(false);
        }
        else{
            next.SetEnabled(true);
        }
        if (page <= 1){
            back.SetEnabled(false);
        }
        else{
            back.SetEnabled(true);
        }


        back.RegisterCallback<ClickEvent>(flipPageBackwards);
        next.RegisterCallback<ClickEvent>(flipPageFordwards);
        play.RegisterCallback<ClickEvent>(transitionEnd);

        
        
    }

    void Update(){
        background.transform.rotation *= Quaternion.Euler(0f, 0f, -5 * Time.deltaTime);
        loadPage();
        if (trasitionTimeCounter > 0){
            trasitionTimeCounter -= Time.deltaTime;
            if (music.volume > 0){
                music.volume -= Time.deltaTime;
            }
            else{
                music.volume = 0;
            }
            
            if(trasitionTimeCounter <= 0){
                loadLevel();
            }
        }
    }

    private void transitionStart(){
        transition.RemoveFromClassList("transition_start");
    }
    private void transitionEnd(ClickEvent clickEvent){
        playSFX.Play();
        transition.AddToClassList("transition_start");
        trasitionTimeCounter = trasitionTimeValue;
    }

    private void loadLevel(){
        if (page == 1){
            SceneManager.LoadSceneAsync("0_Tutorial");
        }
        if (page == 2){
            SceneManager.LoadSceneAsync("1_Level");
        }
    }

    private void flipPageFordwards(ClickEvent clickEvent){
        arrowSFX.Play();
        page ++;

        if (page >= numPages){
            next.SetEnabled(false);
        }
        else{
            next.SetEnabled(true);
        }
        if (page <= 1){
            back.SetEnabled(false);
        }
        else{
            back.SetEnabled(true);
        }

        loadPage();
    }

    private void flipPageBackwards(ClickEvent clickEvent){
        arrowSFX.Play();
        page --;

        if (page >= numPages){
            next.SetEnabled(false);
        }
        else{
            next.SetEnabled(true);
        }
        if (page <= 1){
            back.SetEnabled(false);
        }
        else{
            back.SetEnabled(true);
        }

        loadPage();
    }

    private void loadPage(){
      
        LevelData elNivel = checkPagelevel();
        
        levelName.text = elNivel.getName();
        if (elNivel.getBestTime() > 0){
            score.text = calculateScore(elNivel);
        }
        else{
            score.text = "---";
        }
        

        if (elNivel.getItem1()){
            item1.style.backgroundImage = Background.FromTexture2D(apple);
        }
        else{
            item1.style.backgroundImage = Background.FromTexture2D(missing);
        }

        if (elNivel.getItem2()){
            item2.style.backgroundImage = Background.FromTexture2D(ballon);
        }
        else{
            item2.style.backgroundImage = Background.FromTexture2D(missing);
        }

        if (elNivel.getItem3()){
            item3.style.backgroundImage = Background.FromTexture2D(canvas);
        }
        else{
            item3.style.backgroundImage = Background.FromTexture2D(missing);
        }

        if (elNivel.getItem4()){
            item4.style.backgroundImage = Background.FromTexture2D(train);
        }
        else{
            item4.style.backgroundImage = Background.FromTexture2D(missing);
        }

        if (elNivel.getItem5()){
            item5.style.backgroundImage = Background.FromTexture2D(fly);
        }
        else{
            item5.style.backgroundImage = Background.FromTexture2D(missing);
        }

        if (page == 1){
            image.style.backgroundImage = Background.FromTexture2D(tutorial);
        }
        else if (page == 2){
            image.style.backgroundImage = Background.FromTexture2D(level1);
        }


        if(elNivel.getBestTime() > 0){
            time.text = formatTime(elNivel.getBestTime());
        }
        else{
            time.text = "---";
        }
        
        if (elNivel.getBestTime() <= 0){
            trophy.style.backgroundImage = Background.FromTexture2D(missing);
        }
        else if (elNivel.getBestTime() < GameData.TutorialGold){
            Debug.Log("gold");
            trophy.style.backgroundImage = Background.FromTexture2D(gold);
        }
        else if (elNivel.getBestTime() < GameData.TutorialSilver){
            Debug.Log("silver");
            trophy.style.backgroundImage = Background.FromTexture2D(silver);
        }
        else if (elNivel.getBestTime() < GameData.TutorialBronze){
            Debug.Log("bronze");
            trophy.style.backgroundImage = Background.FromTexture2D(bronze);
        }
        


        int theRank = elNivel.getRank();

        switch(theRank){
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
            case -1:
                rank.style.backgroundImage = Background.FromTexture2D(missing);
                break;
            
        }
    }

    private string calculateScore(LevelData level){
        int points = level.getScore();
        int bonus = 0;
        if (level.getItem1()){
            bonus += 10000;
        }
        if (level.getItem2()){
            bonus += 10000;
        }
        if (level.getItem3()){
            bonus += 10000;
        }
        if (level.getItem4()){
            bonus += 10000;
        }
        if (level.getItem5()){
            bonus += 10000;
        }

        int timeBonus = 0;
        int value = 300 - Convert.ToInt32(level.getBestTime());
        if (value > 0){
            timeBonus += value * 200;
        }

        int totalScore = points + bonus + timeBonus;
        Debug.Log(totalScore);
        return totalScore.ToString();
    }

    private LevelData checkPagelevel(){
        if (page == 1){
            return GameData.getTutorial();
        }
        if (page == 2){
            return GameData.getLevel1();
        }
        
        return null;
    }

    private String formatTime(float timer){

        int minutos;
        int segundos;
        int decimas;
        

        minutos = (int) (timer / 60f);
        segundos = (int) (timer - minutos * 60f);
        decimas = (int) ((timer - (int)timer) * 100f);

        return String.Format("{0:00}:{1:00}:{2:00}", minutos, segundos, decimas);
    }
}
