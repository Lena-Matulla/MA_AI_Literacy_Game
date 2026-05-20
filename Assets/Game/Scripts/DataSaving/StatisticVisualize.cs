using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatisticVisualize : MonoBehaviour
{
    [SerializeField] 
    public TMP_Text accuracyV;
    public TMP_Text accuracyHumanV;
    public TMP_Text accuracyAnimalV;
    public TMP_Text accuracyArchitectureV;
    public TMP_Text accuracyTextV;
    public TMP_Text accuracyRealV;
    public TMP_Text accuracyFakeV;
    public TMP_Text SusBiasV;
    public TMP_Text FalsePosV;
    public TMP_Text FalseNegV;
    public TMP_Text AvgConfidenceV;
    public TMP_Text AvgConfidenceCorrectV;
    public TMP_Text AvgConfidenceIncorrectV;

    
    public void UpdateData(float accuracy, Dictionary<string, float> accPerGroup, Dictionary<string,float> accRealFake, float susBias, float falsePosRate, float falseNegRate, float avConfidence, Dictionary<string, float> avConfByCorrectness)
    {
        accuracyV.SetText(accuracy.ToString("F2") + "%");
        accuracyHumanV.SetText(accPerGroup["Human"].ToString("F2") + "%");
        accuracyAnimalV.SetText(accPerGroup["Animals"].ToString("F2") + "%");
        accuracyArchitectureV.SetText(accPerGroup["Architecture"].ToString("F2") + "%");
        accuracyTextV.SetText(accPerGroup["Text"].ToString("F2") + "%");

        accuracyRealV.SetText(accRealFake["real"].ToString("F2") + "%");
        accuracyFakeV.SetText(accRealFake["fake"].ToString("F2") + "%");

        SusBiasV.SetText(susBias.ToString("F2"));
        FalsePosV.SetText(falsePosRate.ToString("F2") + "%");
        FalseNegV.SetText(falseNegRate.ToString("F2") + "%");

        AvgConfidenceV.SetText(avConfidence.ToString("F2"));

        AvgConfidenceCorrectV.SetText(avConfByCorrectness["correct"].ToString("F2"));
        AvgConfidenceIncorrectV.SetText(avConfByCorrectness["incorrect"].ToString("F2"));

    }



}
