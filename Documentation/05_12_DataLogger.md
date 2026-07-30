# DataLogger.cs

## Purpose

Class to log all the image data and informations in the csv.

Creates one trial log file per player and adds every completed trial as a new line.

## Used By

TrialManager

## Inspector References

/

## Important Methods

### Awake()

Gets the PlayerId from the GameConfigManager.

Gets the SessionId and session start time from the SessionIDManager.

Stores the session start time as a string with the format:

yyyy-MM-dd HH:mm:ss

Creates the Logs directory inside Application.persistentDataPath if it does not exist.

Sets the file name to:

player_{PlayerId}_trials.csv

Calls WriteHeaderIfNeeded().

### WriteHeaderIfNeeded()

Checks if the header was already handled during the current runtime.

If the csv file does not exist or is empty, it creates the header.

The header contains:

* player ID
* session ID
* session start date
* session duration
* trial index
* media name
* ground truth
* user choice
* accuracy
* reaction time
* local marked position
* normalized marked position
* if the image was marked
* confidence
* amount of Real button clicks
* amount of Fake button clicks
* explanation text
* current category
* media type

After the check, _headerWritten is set to true.

### LogTrial(int trialIndex, string imageName, bool groundTruthIsFake, bool userChoseFake, int accuracy, int reactionTimeMs, Vector2 lastLocal, Vector2 lastNormal, bool imageMarked, float confidence, int realclicked, int fakeclicked, string whyText, string currentCategory, MediaType mediaType)

Calls WriteHeaderIfNeeded().

Converts the ground truth and user choice bool values into real or fake strings.

Calculates the current session duration in milliseconds by comparing the current time with the session start time.

Makes the new line for the csv and fills it with all the needed variables.

Each trial is a new line and contains:

* PlayerId
* SessionId
* session start time
* current session duration in ms
* trial index
* image or video filename
* ground truth
* user choice
* accuracy
* reaction time in ms
* local X and Y position
* normalized X and Y position
* imageMarked
* confidence
* Real button clicks
* Fake button clicks
* whyText
* currentCategory
* mediaType

Float values are converted with CultureInfo.InvariantCulture so that a dot is always used as decimal separator.

Strings which could cause csv syntax problems are passed through EscapeCsv().

The completed line is then added to the player csv file.

Which data is stored can also be seen in:

"06_DataLogging.md"

### EscapeCsv(string value)

This function ensures that there are no syntax problems with the csv.

It makes sure that:

* if sth is null or empty it fills it with ""
* if "," , """ , "\n" or "\r" is in the string, it sets it into quotes ""
* quotation marks inside the value are doubled

Returns the escaped string which can safely be written into the csv.

## Data

Stores the trial data inside:

Application.persistentDataPath/Logs/player_{PlayerId}_trials.csv

All sessions of the same player are added to the same csv file.

The SessionId, session start time and session duration can be used to separate the different sessions inside the file.

## Notes

The csv header is only written if the file is new or empty.

The Session duration represents the time from the start of the session until the current trial was logged.

Ground truth and user choice are stored as real or fake strings.

The MediaType is stored so that images and videos can be differentiated.

PlayerId is directly used inside the filename and is not made filename-safe inside this script.
