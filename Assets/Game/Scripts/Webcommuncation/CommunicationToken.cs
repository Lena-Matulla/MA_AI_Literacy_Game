using System;
using System.IO;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static UnityEngine.Rendering.STP;
[Serializable]
public class CommunicationToken
{
    public string player_id;
    public string password;
    public string logfile;
    public string config;
    public int points;
    public int level;
    public int playthroughs;

    public void LoadData()
    {
        // Todo: Echte Inhalte auslesen!
        /*
        player_id = "Player01";
        password = "secret";
        logfile = "log.txt";
        config = "default";
        points = 150;
        level = 3;
        playthroughs = 5;
           */

        GameProgressData progress = SaveManager.Load();

        player_id = GameConfigManager.Config.playerID;
        password = GameConfigManager.Config.password;

        //set logfile
        string logsDir = Path.Combine(Application.persistentDataPath, "Logs");
        string logPath = Path.Combine(logsDir, $"player_{player_id}_trials.csv");
        if (File.Exists(logPath))
        {
            logfile = File.ReadAllText(logPath);
        }
        else
        {
            logfile = "";
            Debug.LogWarning("No logfile found at: " + logPath);
        }

        //set config
        string configPath = Path.Combine(Application.persistentDataPath, "config.json");

        if (File.Exists(configPath))
        {
            config = File.ReadAllText(configPath);
        }
        else
        {
            config = "";
            Debug.LogWarning("No config file found at: " + configPath);
        }

        points = progress.currentPoints;
        level = progress.currentLevel;
        playthroughs = progress.playthroughs;

    }
    
}
