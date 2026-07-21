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
        accuracyV.SetText(accuracy.ToString("F0") + "%");
        //accuracyHumanV.SetText(accPerGroup["Human"].ToString("F0") + "%");
        //accuracyAnimalV.SetText(accPerGroup["Animals"].ToString("F0") + "%");
        //accuracyArchitectureV.SetText(accPerGroup["Architecture"].ToString("F0") + "%");
        //accuracyTextV.SetText(accPerGroup["Text"].ToString("F0") + "%");

        accuracyRealV.SetText(accRealFake["real"].ToString("F0") + "%");
        accuracyFakeV.SetText(accRealFake["fake"].ToString("F0") + "%");

        SusBiasV.SetText(susBias.ToString("F0") + "%");
        FalsePosV.SetText(falsePosRate.ToString("F0") + "%");
        FalseNegV.SetText(falseNegRate.ToString("F0") + "%");

        AvgConfidenceV.SetText(avConfidence.ToString("F1") + "/10");

        AvgConfidenceCorrectV.SetText(avConfByCorrectness["correct"].ToString("F1") + "/10");
        AvgConfidenceIncorrectV.SetText(avConfByCorrectness["incorrect"].ToString("F1") + "/10");

    }



}
