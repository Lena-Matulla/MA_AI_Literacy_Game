# GameConfigManager.cs

## Purpose
Class to create,load and adjust the config file of the project.

## Used By
/

## Inspector References
/

## Classes 
- GameConfig
  - Container class which gives the elements of the config file
    - playerID
    - password
    - experimentID
    - serverURL
    - createdAt
- GameConfigManager
  - The class thorugh which the config file can be created, loaded and changed

## Important Methods

### LoadOrCreateConfig()
If there is already a config file, it calls LoadConfig().
If there isn't one, it creates a new config with default values and calls SaveConfig().

### LoadConfig()
Called when there is already a config file. Loads the Config file out of the json file. Then checks if one of the variables is empty, it inserts dummy values if that is the case.
In the end it calls SaveConfig() if changes where made

## SaveConfig()
Saves the config again in a json file at the correct location.

## SetExperimentID(string experimentID), SetServerUrl(string serverURL)
Both are functions to change the variables (Experiment ID, ServerURL) of the config 

## ResetPlayerID()
Can be called to reset the PlayerID in the config.

## Data
/

## Notes