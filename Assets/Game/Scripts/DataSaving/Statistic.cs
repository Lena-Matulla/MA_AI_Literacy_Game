using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Statistic : MonoBehaviour
{
    public CSVLogParser csvLogParser;

    public List<CSVDataForStatistics> data = new List<CSVDataForStatistics>();

    [SerializeField] private StatisticVisualize statisticVisualize;

    void Start()
    {
        //LoadData();
        
    }

    public void LoadData()
    {
        List<string> categories = new List<string>{"Animals","Human","Architecture","Text"};
        List<string> realFake = new List<string> { "fake", "real"};
        List<string> state = new List<string> { "correct", "incorrect"};


        data = csvLogParser.Load();
        Debug.Log("Loaded trials: " + data.Count);

        float accuracy = CalculateAccuracy(data);
        Debug.Log("Accuracy: " + accuracy + "%");

        Dictionary<string, float> dic = CalculateAccuracyPerGroup(data);
        foreach (string categorie in categories)
        {
            if (dic.ContainsKey(categorie))
            {
                Debug.Log(categorie + ": " + dic[categorie]);
            }
            else
            {
                dic[categorie] = 0;
                Debug.Log(categorie + ": No Data Yet");
            }
        }

        Dictionary<string, float> dicRF = CalculateAccuracyRealFake(data);
        foreach(string s in realFake)
        {
            if (dicRF.ContainsKey(s))
            {
                Debug.Log(s + ": " + dicRF[s]);
            }
            else
            {
                dicRF[s] = 0;
                Debug.Log(s + ": No Data Yet");
            }
        }

        float SusBias = CalculateSuspicionBias(data);
        Debug.Log("SusBias: " + SusBias);
        
        float falsePosRate = CalculateFalsePositiveOrNegativeRate(data, true);
        Debug.Log("False Positive Rate: " + falsePosRate.ToString("F2") + "%");
        

        float falseNegRate = CalculateFalsePositiveOrNegativeRate(data, false);
        Debug.Log("False Negative Rate: " + falseNegRate.ToString("F2") + "%");

        float averConfidence = CalculateAverageConfidence(data);
        Debug.Log("Average Confidence: " +  averConfidence );

        Dictionary<string,float> dicConf = CalculateConfidenceByCorrectness(data);
        foreach(string st in state)
        {
            if (dicConf.ContainsKey(st))
            {
                Debug.Log(st + ": " + dicConf[st]);
            }
            else
            {
                dicConf[st] = 0;
                Debug.Log(st + ": No Data Yet");
            }
        }

        statisticVisualize.UpdateData(accuracy,dic,dicRF,SusBias,falsePosRate,falseNegRate,averConfidence, dicConf);  
    }

    // Overall Accuracy
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

    // Accuracy per Category
    private Dictionary<string, float> CalculateAccuracyPerGroup(List<CSVDataForStatistics> trials)
    {
        Dictionary<string,float> accuracyGroup = new Dictionary<string,float>();

        if (trials.Count == 0) return accuracyGroup;



        var grouped = trials.GroupBy(t => t.currentCategory);

        foreach (var group in grouped)
        {
            float acc = CalculateAccuracy(group.ToList());
            accuracyGroup[group.Key] = acc;
        }

        return accuracyGroup;
    }

    // Accuracy per Fake/Real
    private Dictionary<string,float> CalculateAccuracyRealFake(List<CSVDataForStatistics> trials)
    {
        Dictionary<string, float> accuracyRF = new Dictionary<string, float>();
        if (trials.Count == 0) return accuracyRF;

        var grouped = trials.GroupBy(t => t.groundTruthIsFake);
        foreach (var group in grouped)
        {
            float acc = CalculateAccuracy(group.ToList());
            //have to turn around, because if it is true it is fake
            string key = group.Key ? "fake" : "real";
            accuracyRF[key] = acc;
        }

        return accuracyRF;
    }

    //Bias

    //Suspicion Bias -> General distrust?
    //times user selected fake / all trials

    private float CalculateSuspicionBias(List<CSVDataForStatistics> trials)
    {
        float susBias = 0;
        int selectedFake = 0;
        if (trials.Count == 0) return susBias;

        var grouped = trials.GroupBy(t => t.userchoice);
        //true = chose fake
        var fakeGroup = grouped.FirstOrDefault(group => group.Key == true);

        if(fakeGroup != null)
        {
            selectedFake = fakeGroup.Count();
        }

        susBias = (float)selectedFake / trials.Count();
        return susBias;

    }

    // falsePositive = true:
    // False Positives -> Real images incorrectly judged as fake
    // Rate = (real image + user selected fake) / all real images * 100
    // Out of all real images, how many were falsly accused of being fake?

    // falsePositive = false:
    // False Negatives -> Fake images incorrectly judged as real
    // Rate = (fake image + user selected real) / all fake images * 100
    // Out of all fake images, how many were missed?
    private float CalculateFalsePositiveOrNegativeRate(List<CSVDataForStatistics> trials, bool falsePositive)
    {
        float result = 0;
        int overallCounter = 0;
        int selectedCounter = 0;

        foreach(var trial in trials)
        {
            if(trial.groundTruthIsFake == !falsePositive)
            {
                overallCounter++;
                if (trial.userchoice == falsePositive)
                {
                    selectedCounter++;
                }
                
            }
        }

        result = (float)selectedCounter / overallCounter * 100;

        return result;
    }

  
    //Confidence:

    // Average Confidence
    private float CalculateAverageConfidence(List<CSVDataForStatistics> trials)
    {
        float result = 0;
        float confidence = 0;

        foreach (var trial in trials)
        {
            confidence += trial.confidence;
        }

        result = confidence / trials.Count();

        return result;
    }

    // Correct Confidence
    // Confidence on trials where chosen correct
    private Dictionary<string, float> CalculateConfidenceByCorrectness(List<CSVDataForStatistics> trials)
    {
        var grouped = trials.GroupBy(t => t.accuracy);
        Dictionary<string,float> result = new Dictionary<string,float>();
        
        foreach (var acc in grouped)
        {
            float confidence = 0;
            
            foreach(var trial in acc)
            {
                confidence += trial.confidence;
            }

            // acc.Key == 1 means correct trials
            string key = acc.Key == 1 ? "correct" : "incorrect";

            result[key]= confidence / acc.Count();
        }

        return result;
    }
}
