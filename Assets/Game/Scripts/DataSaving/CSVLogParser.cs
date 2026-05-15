using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
public class CSVDataForStatistics
{
    public string playerID;
    public string sessionID;
    public string sessionStartTime;
    public int sessionDurationMS;
    public int trialIndex;
    public string imageName;
    public int groundTruthIsFake;
    public bool userchoice;
    public int accuracy;
    public int reactionTimeMS;
    public Vector2 lastLocal;
    public Vector2 lastNormal;
    public bool togglechecked;
    public float confidence;
    public int realclicked;
    public int fakeclicked;
    public string whyText;
    public string currentCategory;
}

public class CSVLogParser : MonoBehaviour
{
    public List<CSVDataForStatistics> data = new List<CSVDataForStatistics>();
    string filePath;
    private void Start()
    {
        string logsDir = Path.Combine(Application.persistentDataPath, "Logs");
        filePath = Path.Combine(logsDir, $"player_{GameConfigManager.Config.playerID}_trials.csv");

        data = LoadData(filePath);

    }

    public List<CSVDataForStatistics> Load()
    {
        return LoadData(filePath);
    }

    private List<CSVDataForStatistics> LoadData(string filePath)
    {
        List<CSVDataForStatistics> loadedData = new List<CSVDataForStatistics>();
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("CSV file not found: " + filePath);
            return loadedData;
        }

        string[] lines = File.ReadAllLines(filePath);

        //header is skipped
        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
            { continue; }

            string[] values = lines[i].Split(',');

            CSVDataForStatistics trial = new CSVDataForStatistics();

            trial.playerID = values[0];
            trial.sessionID = values[1];
            trial.sessionStartTime = values[2];
            trial.sessionDurationMS = int.Parse(values[3]);
            trial.trialIndex = int.Parse(values[4]);
            trial.imageName = values[5];
            trial.groundTruthIsFake = int.Parse(values[6]);
            trial.userchoice = bool.Parse(values[7]);
            trial.accuracy = int.Parse(values[8]);
            trial.reactionTimeMS = int.Parse(values[9]);
            trial.lastLocal = ParseVec2(values[10]);
            trial.lastNormal = ParseVec2(values[11]);
            trial.togglechecked = bool.Parse(values[12]);
            trial.confidence = float.Parse(values[13]);
            trial.realclicked = int.Parse(values[14]);
            trial.fakeclicked = int.Parse(values[15]);
            trial.whyText = values[16];
            trial.currentCategory = values[17];

            loadedData.Add(trial);
        }

        return loadedData;
    }

    private Vector2 ParseVec2(string value)
    {
        value = value.Replace("(", "").Replace(")", "");

        string[] parts = value.Split(',');

        float x = float.Parse(parts[0], CultureInfo.InvariantCulture);
        float y = float.Parse(parts[1], CultureInfo.InvariantCulture);

        return new Vector2(x, y);
    }

}
