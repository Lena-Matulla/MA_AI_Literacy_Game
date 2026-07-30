# ProgressManager.cs

## Purpose

Processes the progress of Points and Levels.

Also handles the effects when a new level is reached, saves and loads the progress and updates the office progress visuals.

aka:
Point and Level System

## Used By

Scripts which add or remove TrustValue points, for example TrialManager.

## Inspector References

| Field                  | Type                | Purpose                                                                                                                        |
| ---------------------- | ------------------- | ------------------------------------------------------------------------------------------------------------------------------ |
| Current Points         | int                 | the counter for the current TrustValue points inside the current level                                                         |
| Current Level          | int                 | the counter for the current Level                                                                                              |
| Level Thresholds       | int[]               | array which holds the amount of points needed to progress from each level into the next level and defines the amount of levels |
| Office Visuals         | UpdateOfficeVisuals | reference to the UpdateOfficeVisuals script for the visual update after adding/removing points and at start                    |
| Notebook               | GameObject          | reference to the Notebook which is activated when a new level is reached                                                       |
| Book                   | Book                | reference to the Book which unlocks and opens the new text pages after a level up                                              |
| Level Up PopUp Manager | PopUpManager        | reference to the LevelUp Popup which is played after a new level is reached                                                    |

## Important Methods

### Awake()

Checks if another ProgressManager instance already exists.

If another instance exists, this GameObject is destroyed.

Otherwise the instance is set, DontDestroyOnLoad() is called and LoadProgress() is called.

### Start()

RefreshVisuals() is called.

### AddPoints(int amount)

Called when points should be added, amount dedicates how much are added.

Points are updated and the following functions are called:

UpdateLevel(), SaveProgress() and RefreshVisuals().

### RemovePoints(int amount)

same as above only with removing points.

If the points would become lower than zero, they are set to zero.

Then UpdateLevel(), SaveProgress() and RefreshVisuals() are called.

Right now not used.

### UpdateLevel()

calls GetLevelFromPoints(currentPoints).

### GetLevelFromPoints(int points)

This method checks if the points are enough to move on to the next level and then calculates the leftover points which are already added to the counter for the next level.

while loop because in case there are more points than needed to achieve the new level, the leftover points are checked again if they are already enough for the next level as well.

When a new level is reached:

* the needed points are removed from currentPoints
* currentLevel is increased
* the Notebook is activated
* the Book unlocks all text pages until the new current level
* the Book opens at the newest unlocked text
* the LevelUp Popup animation is played

Also covers the case when the final level is reached.

At the final level, currentPoints cannot become higher than the final value inside levelThresholds.

The points parameter is currently not directly used inside the method. The method works with currentPoints.

### SaveProgress()

Loads the GameProgressData container.

Updates currentPoints and currentLevel inside the data and then saves it with the SaveManager.Save(data) call.

### LoadProgress()

Loads the saved points and level with the SaveManager class.

### RefreshVisuals()

Calls the UpdateOfficeVisuals class to update the visuals based on the current points and level.

If the final level is not reached, the next level threshold is used as the maximum amount for the progress bar.

At the final level, the last level threshold is used.

Only updates the visuals if officeVisuals is not null.

### DebugAdd10Points()

Debug function to add 10 points through the Inspector Context Menu.

### ResetProgress()

Resets currentPoints and currentLevel to zero.

Deletes the current save file by calling SaveManager.DeleteSave().

Then saves the fresh progress data and updates the UI by calling RefreshVisuals().

Currently used through the Inspector Context Menu.

## Data

Stores the current TrustValue points and the current level.

The progress is stored inside GameProgressData through the SaveManager.

The current points represent the progress inside the current level. When a level is reached, the needed points are removed and the remaining points are carried into the next level.

## Notes

ProgressManager uses a singleton instance and is not destroyed when a new scene is loaded.

The amount of available levels is based on the length of levelThresholds.

The first threshold is zero and the following values define how many points are needed for the next level.

RemovePoints() does not currently reduce currentLevel. It only removes points from the current level.

The GetLevelFromPoints(int points) parameter is currently unused.
