# StatisticVisualize.cs

## Purpose
Displays the calculated statistic values on the tablet statistic overview.

## Used By
Statistic

## Inspector References
| Field | Type | Purpose |
|---|---|---|
| accuracyV | TMP_Text | Reference to the textfield on the tablet UI regarding Accuracy |
| accuracyHumanV | TMP_Text | Reference to the textfield on the tablet UI regarding Accuracy of the Human Category |
| accuracyAnimalV | TMP_Text | Reference to the textfield on the tablet UI regarding Accuracy of the Animal Category |
| accuracyArchitectureV | TMP_Text | Reference to the textfield on the tablet UI regarding Accuracy of the Architecture Category |
| accuracyTextV | TMP_Text | Reference to the textfield on the tablet UI regarding Accuracy of the Text Category |
| accuracyRealV | TMP_Text | Reference to the textfield on the tablet UI regarding Accuracy of Real Images |
| accuracyFakeV | TMP_Text | Reference to the textfield on the tablet UI regarding Accuracy of Fake Images |
| SusBiasV | TMP_Text | Reference to the textfield on the tablet UI regarding Suspicion Bias |
| FalsePosV | TMP_Text | Reference to the textfield on the tablet UI regarding False Positive Rate |
| FalseNegV | TMP_Text | Reference to the textfield on the tablet UI regarding False Negative Rate |
| AvgConfidenceV | TMP_Text | Reference to the textfield on the tablet UI regarding average Confidence |
| AvgConfidenceCorrectV | TMP_Text | Reference to the textfield on the tablet UI regarding average Confidence of correcty decided images |
| AvgConfidenceIncorrectV | TMP_Text | Reference to the textfield on the tablet UI regarding average Confidence of incorrecty decided images |



## Important Methods

### UpdateData(float accuracy, Dictionary<string, float> accPerGroup, Dictionary<string,float> accRealFake, float susBias, float falsePosRate, float falseNegRate, float avConfidence, Dictionary<string, float> avConfByCorrectness)
Gets all the needed values from the Statistic.cs script.
Sets the texts to the new values. Adds "%" and "/10" if needed.

## Data
/

## Notes


