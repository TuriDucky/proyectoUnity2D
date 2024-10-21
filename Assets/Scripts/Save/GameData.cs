using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class GameData : MonoBehaviour
{
    public static LevelData tutorial;
    void Start(){
        loadSaveFile();
    }

    public void loadSaveFile(){
        if (File.Exists("save.txt")){
            
        }
        else{ 
            File.WriteAllText("save.txt", createSaveFile());
        }
        loadSaveData();
    }

    public String createSaveFile(){
        String savedata = "";

        savedata += "Tutorial|"; //Nombre del nivel
        savedata += "false|"; //Si esta completado o no
        savedata += "000000|"; //El mejor tiempo
        savedata += "false|"; //Collecionable 1
        savedata += "false|"; //Collecionable 2
        savedata += "false|"; //Collecionable 3
        savedata += "false|"; //Collecionable 4
        savedata += "false|"; //Collecionable 5

        return savedata;
    }

    public void loadSaveData(){
        String fileData = File.ReadAllText("save.txt");
        String[] data = fileData.Split("|");
        tutorial = new LevelData(data[0], Convert.ToBoolean(data[1]), float.Parse(data[2]), Convert.ToBoolean(data[3]), Convert.ToBoolean(data[4]), Convert.ToBoolean(data[5]), Convert.ToBoolean(data[6]), Convert.ToBoolean(data[7]) );
    }

    public static void saveLevelData(LevelData level){
        String file = File.ReadAllText("save.txt");
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
        
        File.Delete("save.txt");
        File.WriteAllText("save.txt", String.Concat(data));
    }

    public static LevelData getTutorial(){
        return tutorial;
    }
}
