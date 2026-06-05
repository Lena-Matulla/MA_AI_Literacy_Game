using UnityEngine;
using System;
using System.IO;


//class which is the structure of all the Configurations which are stored
[Serializable]
public class GameConfig
{
    public string playerID;
    public string experimentID;
    public string serverURL;
    public string createdAt;
}
public class GameConfigManager
{
    private static string ConfigPath => Path.Combine(Application.persistentDataPath, "config.json");

    public static GameConfig Config { get; private set; }

    //create new config
    public static void LoadOrCreateConfig()
    {
        if (File.Exists(ConfigPath))
        {
            LoadConfig();
        }
        else
        {
            Config = new GameConfig
            {
                playerID = "",//Guid.NewGuid().ToString(),
                experimentID = "EXP_001",
                serverURL = "default",
                createdAt = DateTime.UtcNow.ToString("o")
            };

            SaveConfig();
        }
    }

    //Load config out of json
    public static void LoadConfig()
    {
        string json = File.ReadAllText(ConfigPath);
        Config = JsonUtility.FromJson<GameConfig>(json);

        //check to see if we change sth
        bool changed = false;

        if (string.IsNullOrWhiteSpace(Config.playerID))
        {
            Config.playerID = "";//Guid.NewGuid().ToString();
            changed = true;
        }

        if (string.IsNullOrWhiteSpace(Config.experimentID))
        {
            Config.experimentID = "EXP_001";
            changed = true;
        }

        if (string.IsNullOrWhiteSpace(Config.serverURL)) 
        {
            Config.serverURL = "default";
            changed = true;
        }

        if (changed)
        {
            SaveConfig();
        }

    }

    public static void SaveConfig()
    {
        string json = JsonUtility.ToJson(Config, true);
        File.WriteAllText(ConfigPath, json);
    }

    public static void SetPlayerID(string playerID)
    {
        Config.playerID = playerID;
        SaveConfig();
    }

    public static void SetExperimentID(string experimentID)
    {
        Config.experimentID = experimentID;
        SaveConfig();
    }

    public static void SetServerUrl(string serverURL)
    {
        Config.serverURL = serverURL;
        SaveConfig();
    }

    public static void ResetPlayerID()
    {
        Config.playerID = "";//"Guid.NewGuid().ToString()";
        SaveConfig();
    }
}
