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
    public static string levelTutorialName = "Tutorial";
    public static float TutorialGold;
    public static float TutorialSilver;
    public static float TutorialBronze;

    public static LevelData Level1;
    public static string level1Name = "Islands";
    public static float Level1Gold;
    public static float Level1Silver;
    public static float Level1Bronze;
    void Start(){
        TutorialGold = 30;
        TutorialSilver = 50;
        TutorialBronze = 70;

        Level1Gold = 140;
        Level1Silver = 190;
        Level1Bronze = 270;
        loadSaveFile();
    }

    public static void loadSaveFile(){
        if (!File.Exists(String.Concat(levelTutorialName, ".txt"))){
            File.WriteAllText(String.Concat(levelTutorialName, ".txt"), createLevelSaveFile(levelTutorialName));
        }
        if (!File.Exists(String.Concat(level1Name, ".txt"))){
            File.WriteAllText((String.Concat(level1Name, ".txt")), createLevelSaveFile(level1Name));
        }
        
        loadSaveData();
    }

    public static String createSaveFile(){
        String savedata = "";

        return savedata;
    }
    public static  String createLevelSaveFile(String levelName){
        String savedata = "";

        savedata += String.Concat(levelName,"|"); //Nombre del nivel (El del Parametro)
        savedata += "false|"; //Si ha sido completado o no
        savedata += "000000|"; //El mejor tiempo
        savedata += "false|"; //Collecionable 1
        savedata += "false|"; //Collecionable 2
        savedata += "false|"; //Collecionable 3
        savedata += "false|"; //Collecionable 4
        savedata += "false|"; //Collecionable 5
        savedata += "0|"; //Puntuacion Maxima (solo la obtenida por los enemigos)
        savedata += "-1|"; //Rango del nivel (D - S)

        return savedata;
    }

    public static void loadSaveData(){
        String tutorialFileData = File.ReadAllText((String.Concat(levelTutorialName, ".txt")));
        String[] tutorialData = tutorialFileData.Split("|");
        Tutorial = new LevelData(tutorialData[0], Convert.ToBoolean(tutorialData[1]), float.Parse(tutorialData[2]), Convert.ToBoolean(tutorialData[3]), Convert.ToBoolean(tutorialData[4]), Convert.ToBoolean(tutorialData[5]), Convert.ToBoolean(tutorialData[6]), Convert.ToBoolean(tutorialData[7]), Convert.ToInt32(tutorialData[8]), Convert.ToInt32(tutorialData[9]));
    
        String level1FileData = File.ReadAllText((String.Concat(level1Name, ".txt")));
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

    public static void deleteSaves()
    {
        File.Delete((String.Concat(levelTutorialName, ".txt")));
        File.Delete((String.Concat(level1Name, ".txt")));

        loadSaveFile();
    }

    public static LevelData getTutorial(){
        return Tutorial;
    }

    public static LevelData getLevel1(){
        return Level1;
    }
}
