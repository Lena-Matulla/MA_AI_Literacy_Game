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
    public bool groundTruthIsFake;
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

        //data = LoadData(filePath);
        //PrintAllParsedData(data);

    }

    public List<CSVDataForStatistics> Load()
    {
        data =  LoadData(filePath);
        PrintAllParsedData(data);
        return data;
        
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


            //Parses based on function, because if i used "," it could crash at whyText
            List<string> values = ParseCsvLine(lines[i]);

            /* Debugging

            Debug.Log($"Line {i}: values count = {values.Count}");

            for (int v = 0; v < values.Count; v++)
            {
                Debug.Log($"values[{v}] = '{values[v]}'");
            }
            */
            CSVDataForStatistics trial = new CSVDataForStatistics();

            trial.playerID = values[0];
            trial.sessionID = values[1];
            trial.sessionStartTime = values[2];
            trial.sessionDurationMS = int.Parse(values[3]);
            trial.trialIndex = int.Parse(values[4]);
            trial.imageName = values[5];
            trial.groundTruthIsFake = values[6] == "fake";
            trial.userchoice = values[7] == "fake";
            trial.accuracy = int.Parse(values[8]);
            trial.reactionTimeMS = int.Parse(values[9]);
            trial.lastLocal = new Vector2(
                    float.Parse(values[10], CultureInfo.InvariantCulture),
                    float.Parse(values[11], CultureInfo.InvariantCulture)
                );
            trial.lastNormal = new Vector2(
                    float.Parse(values[12], CultureInfo.InvariantCulture),
                    float.Parse(values[13], CultureInfo.InvariantCulture)
                );
            trial.togglechecked = bool.Parse(values[14]);
            trial.confidence = float.Parse(values[15], CultureInfo.InvariantCulture);
            trial.realclicked = int.Parse(values[16]);
            trial.fakeclicked = int.Parse(values[17]);
            trial.whyText = values[18];
            trial.currentCategory = values[19];

            loadedData.Add(trial);
        }

        return loadedData;
    }


    private void PrintAllParsedData(List<CSVDataForStatistics> parsedData)
    {
        Debug.Log($"--- CSV parsed entries: {parsedData.Count} ---");

        for (int i = 0; i < parsedData.Count; i++)
        {
            CSVDataForStatistics trial = parsedData[i];

            Debug.Log(
                $"Entry {i}\n" +
                $"playerID: {trial.playerID}\n" +
                $"sessionID: {trial.sessionID}\n" +
                $"sessionStartTime: {trial.sessionStartTime}\n" +
                $"sessionDurationMS: {trial.sessionDurationMS}\n" +
                $"trialIndex: {trial.trialIndex}\n" +
                $"imageName: {trial.imageName}\n" +
                $"groundTruthIsFake: {trial.groundTruthIsFake}\n" +
                $"userchoice: {trial.userchoice}\n" +
                $"accuracy: {trial.accuracy}\n" +
                $"reactionTimeMS: {trial.reactionTimeMS}\n" +
                $"lastLocal: {trial.lastLocal}\n" +
                $"lastNormal: {trial.lastNormal}\n" +
                $"togglechecked: {trial.togglechecked}\n" +
                $"confidence: {trial.confidence}\n" +
                $"realclicked: {trial.realclicked}\n" +
                $"fakeclicked: {trial.fakeclicked}\n" +
                $"whyText: {trial.whyText}\n" +
                $"currentCategory: {trial.currentCategory}"
            );
        }
    }

    private List<string> ParseCsvLine(string line)
    {
        List<string> values = new List<string>();
        bool insideQuotes = false;
        string currentValue = "";

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                // Handles escaped quotes: ""
                if (insideQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    currentValue += '"';
                    i++;
                }
                else
                {
                    insideQuotes = !insideQuotes;
                }
            }
            else if (c == ',' && !insideQuotes)
            {
                values.Add(currentValue);
                currentValue = "";
            }
            else
            {
                currentValue += c;
            }
        }

        values.Add(currentValue);

        return values;
    }

}
