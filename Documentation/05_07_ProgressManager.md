# ProgressManager.cs

## Purpose
Processes the progress of Points and Levels.

aka:
Point and Level System

## Used By
ProgressManager

## Inspector References
| Field | Type | Purpose |
|---|---|---|
| Current Points | int | the counter for the current TrustValue points |
| Current Level | int | the counter for the current Level |
| Level Threshold | int[] | array which holds the different threshholds needed to progress into the next level|
| Office Visuals | UpdateOfficeVisuals | reference to the OfficeProgressManager for the visual updated after adding/removing points and at start |

## Important Methods

### Awake()
On load the instanze is set and LoadProgress() is called

### Start()
RefreshVisuals() is called

### AddPoints(int amount)
Called when points should be added, amount dedicates how much are added. 
Points are updated and the following functions are called:
UpdateLevel(), SaveProgress() and RefreshVisuals()


### RemovePoints(int amount)
same as above only with removing points.
Right now not used

### UpdateLevel()
calls GetLevelFromPoints(currentPoints)

### GetLevelFromPoints(int points)
This method checks if the points are enough to move on to the next level, and then calculate the leftover points which are already added to the counter for the next level. Also covers the case that when the final level is reached.

while loop because in case there are more points then needed to archieve the new level, the leftover points are checked again if they are already enough for the next level as well.

### SaveProgress()
Saves the data in the GameProgressData container for the data and then saves it with the SaveManager.save(data) call.

### LoadProgress()
Loads the saved points and level with the SaveManager class.

### RefreshVisuals()
calls the OfficeProgressManager class to update the visuals based on the level.

### DebugAdd10Points()
Debug function to add 10 points per inspector

### ResetProgress()
To reset Progress. Currently used per inspector.

## Data


## Notes


