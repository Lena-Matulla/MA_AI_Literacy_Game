using System.Collections.Generic;
using UnityEngine;

public class Statistic : MonoBehaviour
{
    public CSVLogParser csvLogParser;

    public List<CSVDataForStatistics> data = new List<CSVDataForStatistics>();
    void Start()
    {
        //LoadData();
    }

    public void LoadData()
    {
        data = csvLogParser.Load();
        Debug.Log("Loaded trials: " + data.Count);

        float accuracy = CalculateAccuracy(data);
        Debug.Log("Accuracy: " + accuracy + "%");
    }

    private float CalculateAccuracy(List<CSVDataForStatistics> trials)
    {
        if (trials.Count == 0)
            return 0f;

        int correct = 0;

        foreach (CSVDataForStatistics trial in trials)
        {
            if (trial.accuracy == 1)
                correct++;
        }

        return (float)correct / trials.Count * 100f;
    }


}
