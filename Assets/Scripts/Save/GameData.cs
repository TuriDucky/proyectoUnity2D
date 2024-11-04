using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class GameData : MonoBehaviour
{
    public static int numeroNiveles = 1;
    public static LevelData Tutorial;
    public static float TutorialGold;
    public static float TutorialSilver;
    public static float TutorialBronze;

    public static LevelData Level1;
    public static float Level1Gold;
    public static float Level1Silver;
    public static float Level1Bronze;
    void Start(){
        TutorialGold = 25;
        TutorialSilver = 40;
        TutorialBronze = 60;

        Level1Gold = 0;
        Level1Silver = 0;
        Level1Bronze = 0;
        loadSaveFile();
    }

    public void loadSaveFile(){
        if (!File.Exists("save.txt")){
            File.WriteAllText("save.txt", createSaveFile());
        }

        if (!File.Exists("Tutorial.txt")){
            File.WriteAllText("Tutorial.txt", createLevelSaveFile("Tutorial"));
        }
        if (!File.Exists("Level1.txt")){
            File.WriteAllText("Level1.txt", createLevelSaveFile("Level1"));
        }
        
        loadSaveData();
    }

    public String createSaveFile(){
        String savedata = "";

        return savedata;
    }
    public String createLevelSaveFile(String levelName){
        String savedata = "";

        savedata += String.Concat(levelName,"|"); //Nombre del nivel (El del Parametro)
        savedata += "false|"; //Si esta completado o no
        savedata += "000000|"; //El mejor tiempo
        savedata += "false|"; //Collecionable 1
        savedata += "false|"; //Collecionable 2
        savedata += "false|"; //Collecionable 3
        savedata += "false|"; //Collecionable 4
        savedata += "false|"; //Collecionable 5
        savedata += "0|"; //Puntuacion Maxima
        savedata += "-1|"; //Rango del nivel (D - S)

        return savedata;
    }

    public void loadSaveData(){
        String tutorialFileData = File.ReadAllText("Tutorial.txt");
        String[] tutorialData = tutorialFileData.Split("|");
        Tutorial = new LevelData(tutorialData[0], Convert.ToBoolean(tutorialData[1]), float.Parse(tutorialData[2]), Convert.ToBoolean(tutorialData[3]), Convert.ToBoolean(tutorialData[4]), Convert.ToBoolean(tutorialData[5]), Convert.ToBoolean(tutorialData[6]), Convert.ToBoolean(tutorialData[7]), Convert.ToInt32(tutorialData[8]), Convert.ToInt32(tutorialData[9]));
    
        String level1FileData = File.ReadAllText("Level1.txt");
        String[] level1Data = level1FileData.Split("|");
        Level1 = new LevelData(level1Data[0], Convert.ToBoolean(level1Data[1]), float.Parse(level1Data[2]), Convert.ToBoolean(level1Data[3]), Convert.ToBoolean(level1Data[4]), Convert.ToBoolean(level1Data[5]), Convert.ToBoolean(level1Data[6]), Convert.ToBoolean(level1Data[7]), Convert.ToInt32(level1Data[8]), Convert.ToInt32(level1Data[9]));
    
    }

    public static void saveLevelData(LevelData level){
        String file = File.ReadAllText(level.getName() + ".txt");
        String[] data = file.Split("|");
        
        if (level.getBeaten()){
            data[1] = "|true|";
        }
        else{
            data[1] = "|false|";
        }

        data[2] = level.getBestTime().ToString() + "|";

        if(level.getItem1()){
            data[3] = "true|";
        }
        else{
            data[3] = "false|";
        }

        if(level.getItem2()){
            data[4] = "true|";
        }
        else{
            data[4] = "false|";
        }

        if(level.getItem3()){
            data[5] = "true|";
        }
        else{
            data[5] = "false|";
        }

        if(level.getItem4()){
            data[6] = "true|";
        }
        else{
            data[6] = "false|";
        }

        if(level.getItem5()){
            data[7] = "true|";
        }
        else{
            data[7] = "false|";
        }

        if (Convert.ToInt32(data[8]) <= level.getScore()){
            data[8] = level.getScore().ToString() +  "|";
        }
        
        if (Convert.ToInt32(data[9]) <= level.getRank()){
            data[9] = level.getRank().ToString() +  "|";
        }
        
        
        
        File.Delete(level.getName() + ".txt");
        File.WriteAllText(level.getName() + ".txt", String.Concat(data));
    }

    public static LevelData getTutorial(){
        return Tutorial;
    }

    public static LevelData getLevel1(){
        return Level1;
    }
}
