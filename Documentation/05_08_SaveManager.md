# SaveManager.cs

## Purpose

Saves the current Progress in the persistent data path. Therefore the current progress is still there even if the game was closed.

Creates a separate save file for every player based on the player ID from the GameConfigManager.

## Used By

Scripts which save, load or delete GameProgressData, for example ProgressManager and ImgProvider.

## Inspector References

/

## Important Methods

### GetSavePath()

Gets the player ID from GameConfigManager.Config.

If GameConfigManager.Config is null or the player ID is missing, a Debug Error is called and null is returned.

Calls MakeFileNameSafe() to replace invalid filename characters inside the player ID.

Creates the path to the player-specific save file.

The save file structure is:

Application.persistentDataPath/Saves/player_PlayerID_progress.json

### Save(GameProgressData data)

Gets the player-specific save path by calling GetSavePath().

If no valid path could be created, the method returns without saving.

Creates the Saves directory if it does not exist.

Converts the provided GameProgressData object into a formatted json and writes it into the player-specific progress file.

### Load()

Gets the player-specific save path by calling GetSavePath().

If no valid path could be created, it returns a new default GameProgressData object.

Loads the saved data out of the json file if there is one.

If no save file exists for the current player, it returns a new default GameProgressData object.

### DeleteSave()

Gets the player-specific save path by calling GetSavePath().

If no valid path could be created, the method returns.

Deletes the current player's save file if it exists.

### MakeFileNameSafe(string input)

Checks the provided player ID for characters which cannot be used inside a filename.

Replaces all invalid filename characters with an underscore.

Returns the safe version of the player ID.

## Data

Stores the GameProgressData as a json file inside the Saves folder of Application.persistentDataPath.

Every player gets a separate save file based on the player ID from GameConfigManager.Config.

## Notes

SaveManager only contains static methods and does not need an Inspector reference.

A valid GameConfigManager.Config and player ID are needed before saving, loading or deleting progress.

Invalid filename characters inside the player ID are automatically replaced with underscores.

If the player ID changes, a different save file is used.

If no save file exists, Load() does not automatically write a new file. It only returns a new GameProgressData object.
