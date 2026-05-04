# VisualFadeSpawn.cs

## Purpose
Lets objects fade in when enabled instead of being visibly at once.

## Used By
Every spawnable object

## Inspector References
| Field | Type | Purpose |
|---|---|---|
| Fade Time | float | Time it takes from enabled to when it is visible 100% |

## Important Methods
### OnEnable()
When enabled, all children of the object are stored. For each the alpha of the color is set to 0.

### Update()
Each frame, the elapsed time gets updated. if the elapsed time is still smaller then the set faid Time, it iterates over all children and sets their alpha value to the current percentage regarding elapsedtime/fadeTime.

## Data
Updates the visibility of object its attached to and its children.

## Notes
