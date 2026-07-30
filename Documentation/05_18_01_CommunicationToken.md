# CommunicationToken.cs

## Purpose

Container class for the data which is sent through the Webcommunication.

Loads the current player information, progress, logfile and config file into one object.

## Used By

Webcommunication

## Inspector References

/

## Classes

* CommunicationToken

  * serializable container class for the communication data
  * contains:

    * player_id
    * password
    * logfile
    * config
    * points
    * level
    * playthroughs

## Important Methods

### LoadData()

Loads the current GameProgressData by calling SaveManager.Load().

Gets the Player ID and password from GameConfigManager.Config.

Creates the path to the player logfile:

Application.persistentDataPath/Logs/player_{player_id}_trials.csv

If the logfile exists, the complete csv content is stored in logfile.

If no logfile exists, logfile is set to an empty string and a Debug warning is called.

Creates the path to the config file:

Application.persistentDataPath/config.json

If the config file exists, the complete json content is stored in config.

If no config file exists, config is set to an empty string and a Debug warning is called.

Gets the following values from the loaded GameProgressData:

* currentPoints
* currentLevel
* playthroughs

Stores them in points, level and playthroughs.

Prints the amount of playthroughs for debugging.

## Data

Stores all data needed for the communication in one serializable container.

The stored data contains:

* Player ID
* password
* complete player logfile
* complete config file
* current points
* current level
* amount of ImageGame playthroughs

## Notes

The class is marked as Serializable so it can be converted into json.

LoadData() expects GameConfigManager.Config to already be loaded.

The logfile and config are stored as complete strings and not as file paths.

If the logfile or config file does not exist, the corresponding value is set to an empty string.

The commented dummy data is not used.

