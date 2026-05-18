# CSVLogParser.cs

## Purpose
Reads out the csv log files and stores the values inside of a constructor class for better handling. Used for statistics

## Used By
Statsitic

## Inspector References
/

## Classes
- CSVDataForStatistics
  - Container class for the values of the logging
- CSVLogParser
  - Parser for the csv logging file to get the values to use them

## Important Methods

### Start()
gets the path to the current log file

### Load()
public function which can be called.
Calls LoadData(filePath) and stores it in "data" and calls PrintAllParsedData(data) for debugging.

## LoadData(string filePath)
Loads the csv (if ther is one)
It iterates over each line and parses its values with ParseCsvLine(lines[i]). Each time creates a new CSVDataForStatistics object and stores alle the values. Then adds it to loadedData (List of CSVDataForStatistics objects).

## PrintAllParsedData(List<CSVDataForStatistics> parsedData)
Helper function for debugging. prints it out. There to check if the data is loaded correctly

## ParseCsvLine(string line)
Used to make sure that there are no problems with the syntax when parsing.
We use this function instead of just parting the values at "," to make sure that we dont parse incorrectly. That coul happen if for example in a string text there is also a ",".
It checks if a "," is inside of quotes "". If yes, that comma is ignored and not used for data splitting.


## Data
/

## Notes


