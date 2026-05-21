# Statistic.cs

## Purpose
Used to calculate easy statistics with the data of the log files. They are then displayed to the user through the "tablet"

## Used By
Statistic

## Inspector References
| Field | Type | Purpose |
|---|---|---|
| csvLogParser | CSVLogParser | Reference to the LogParser |
| statisticVisualize | StatisticVisualize | Reference to the Statistic visualizer |

## Important Methods

### LoadData()
Calls csvLogParser.load() to load the current log data into "data".

- Calls CalculateAccuracy(data)
- Calls CalculateAccuracyPerGroup(data)
  - iterates over list of each categorie and if one is missing in the dicitonary, it fills it with a dummy value
- Calls Calculate AccuracyRealFake(data)
  - Fills in missing elements in the dict (if real or fake is missing)
- Calls CalculateSuspicionBias(data)
- Calls CalculateFalsePositiveOrNegativeRate(data, true) to get the FalsePositive Rate
- Calls CalculateFalsePositiveOrNegativeRate(data, false) to get the FalseNegative Rate
- Calls averConfidence(data) to get the average confidence
- Calls CalculateConfidenceByCorrectness(data)
  - fills in default value for "correct" or "incorrect" if missing in dictionary

Then in the end Calls statisticVisualize.UpdateData with all the stored values. 
This function is further explained in 
"05_16_StatisticsVisualize.md"

### CalculateAccuracy(List<CSVDataForStatistics> trials)
Calculates the current accuracy for all current trials.

### Dictionary<string, float> CalculateAccuracyPerGroup(List<CSVDataForStatistics> trials)
Calculates how accurate the player was for each image category like animals, humans, architecture, or text.
Returns dictionary with the categorie as key.

### Dictionary<string,float> CalculateAccuracyRealFake(List<CSVDataForStatistics> trials)
Calculates how well the player identified real images compared to fake images.
Returns dictionary with "real"/"fake" as key.

### CalculateSuspicionBias(List<CSVDataForStatistics> trials)
Calculates how often the player tended to judge images as fake.

### CalculateFalsePositiveOrNegativeRate(List<CSVDataForStatistics> trials, bool falsePositive)
- falsePositive = true:
  - False Positives -> Real images incorrectly judged as fake
- falsePositive = false:
  - False Negatives -> Fake images incorrectly judged as real

### CalculateAverageConfidence(List<CSVDataForStatistics> trials)
Calculates the players average confidence level across all decisions.

### CalculateConfidenceByCorrectness(List<CSVDataForStatistics> trials)
Calculates how confident the player was when their decisions were correct versus incorrect.
Returns a dictionary with "correct"/"incorrect" as keys.

## Data / Calculations:
- Accuracy:   How often the players decisions were correct overall.
- Accuracy per Group:   How accurate the player was for each image category like animals, humans, architecture, or text.
- Accuracy per Real/Fake:   How well the player identified real images compared to fake images.
- Suspicion Bias:   How often the player tended to judge images as fake.
- False Positive Rate:  How often the player incorrectly judged real images as fake.
- False Negative Rate:  How often the player incorrectly judged fake images as real.
- Average Confidence:   The players average confidence level across all decisions.
- Average Confidence per Correctness:   How confident the player was when their decisions were correct versus incorrect.

## Notes


