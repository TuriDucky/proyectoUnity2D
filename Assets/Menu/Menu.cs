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
    
    Button play;
    Button back;
    Button next;
    Button exit;
    Label levelName;
    Label score;
    Label rank;

    VisualElement item1;
    VisualElement item2;
    VisualElement item3;
    VisualElement item4;
    VisualElement item5;
    VisualElement image;

    public Texture2D missing;
    public Texture2D apple;
    public Texture2D ballon;
    public Texture2D canvas;
    public Texture2D train;
    public Texture2D fly;
    public Texture2D tutorial;
    public Texture2D level1;
    void OnEnable(){

        page = 1;
        menu = GetComponent<UIDocument>();
        VisualElement root = menu.rootVisualElement;

        play = root.Q<Button>("play");
        back = root.Q<Button>("back"); 
        next = root.Q<Button>("next");
        exit = root.Q<Button>("exit");

        levelName = root.Q<Label>("levelName");
        score = root.Q<Label>("score");
        rank = root.Q<Label>("rank");

        item1 = root.Q<VisualElement>("item1");
        item2 = root.Q<VisualElement>("item2");
        item3 = root.Q<VisualElement>("item3");
        item4 = root.Q<VisualElement>("item4");
        item5 = root.Q<VisualElement>("item5");

        image = root.Q<VisualElement>("image");

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
        play.RegisterCallback<ClickEvent>(loadLevel);

        loadPage();
    }

    private void loadLevel(ClickEvent clickEvent){
        if (page == 1){
            SceneManager.LoadSceneAsync("0_Tutorial");
        }  
    }

    private void flipPageFordwards(ClickEvent clickEvent){
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
        if (elNivel.getScore() > 0){
            score.text = elNivel.getScore().ToString();
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


        int theRank = elNivel.getRank();
        string rankLetter = "?";

        switch(theRank){
            case -1:
                rankLetter =  "?";
                break;
            case 0:
                rankLetter =  "D";
                break;
            case 1:
                rankLetter =  "C";  
                break;
            case 2:
                rankLetter =  "B"; 
                break;
            case 3:
                rankLetter =  "A"; 
                break;
            case 4:
                rankLetter =  "S"; 
                break;
        }

        rank.text = rankLetter;
        
        
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
}
