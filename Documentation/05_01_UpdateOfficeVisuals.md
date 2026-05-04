# UpdateOfficeVisuals.cs

## Purpose
This script has the information about which object group gets revealed in which level. A function in this script is called when there is a level up and the regarding object groups are then set to visible.

## Used By
ProgressManager

## Inspector References
| Field | Type | Purpose |
|---|---|---|
| Unlock Groups | OfficeUnlodckGroup | Structure in which the objectgroups are listed/linked with: groupname (string), requiredLevel (int), targetGroup (GameObject), activeWhenUnlocked (bool) (decideds if the object should be disabled or enabled once the level is reached).
| ProgressBar | Slider | Updates the slider to the new max and current Points|
| LevelText | TextMeshPro UGUI | link to the Level number for updates |

## Classes

- OfficeUnlockGroup: 
  - Class to define the struct of the Objects to be unlocked
  - UpdateOfficeVisuals
    - The actual class which does the updates


## Important Methods
### Display(int currentPoints, int currentLevel, int maxpoints);
This function is called from the ProgressManager once a new level is reached.
It iterates through each group and checks if the required level is reached. If not, then the group is disabled, if yes it is enabled. the Children of the group object stay active, but are only shown once the parent (group) object is enabled as well. Therefore a lot of objects can be enabled in an more efficient way (as a group) rather than linking each by their own in the inspector.

It updates the Leveltext and the progressbar (max value and current points) as well.

## Data
Updates the office visually

## Notes
