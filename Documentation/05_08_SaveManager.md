# SaveManager.cs

## Purpose
Saves the current Progress in the persistend data path. Therefore the current progress is still there even if the game was closed.

## Used By
SaveManager

## Inspector References
/

## Important Methods

### Save(GameProgressData data)
Saves the provided GameProgressData object. writes to progress.json in Application.persistentDataPath.

### Load()
Loads the saved data out of the json in the persistend data path if there is one. If no save file exists, it returns a new default GameProgressData object.

## DeleteSave()
Deletes the current save out of the persistend data path.

## Data
/

## Notes


