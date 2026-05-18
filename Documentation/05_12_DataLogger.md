# DataLogger.cs

## Purpose
Class to log all the image data and informations in the csv

## Used By
ImageGameManager

## Inspector References
/

## Important Methods

### Awake()
gets the PlayerId, SessionId, session start time. Sets the location of the directory where the log should be safed. Sets the file name to "player_{PlayerId}_trials.csv". Calls WriteHeaderIfNeeded()

### WriteHeaderIfNeeded()
creates the header if there is no header yet.

## LogTrial(int trialIndex,string imageName,bool groundTruthIsFake,bool userChoseFake,int accuracy,int reactionTimeMs,Vector2 lastLocal,Vector2 lastNormal,bool toggleChecked,float confidence,int realclicked,int fakeclicked,string whyText,string currentCategory)
Calls WriteHeaderIfNeeded()
Makes the new line for the csv and fills it with all the needed variables. Each trial is a new line (each image).
Which data is stored can be seen in
"06_DataLogging.md"

## EscapeCsv(string value)
This function ensures that there are no syntax problems with the csv.
It makes sure that:
- if sth is null it fills it with ""
- if "," , "\" , "\n" or "/r" is in the string, it sets it into quotes ""


## Data
/

## Notes


