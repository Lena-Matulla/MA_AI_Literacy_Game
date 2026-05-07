# PointSliderVisualUpdater.cs

## Purpose
When points are added to the TrustValue Progressbar, it not just updates to the new value, but a visual transition to the new point value is animated.

## Used By
Slider

## Inspector References
/

## Important Methods
### Awake()
Gets the slider component reference.

### AdjustPoints(int points, int maxpoints)
checks if it is the first time running, if yes, it does not animate but only sets the maxvalue and value to the correct position on the slider.
Checks if a new level was reached.
Starts the coroutine by calling StartCoroutine(AnimateBar(points, maxpoints, lastLevelMax, newLevelreached)). If already a coroutine is running, it stops that.

### AnimateBar(int points, int newMaxPoints, int oldMaxPoints, bool newLevelreached)
checks if a new level was reached.
If yes, it first fills the bar completly by calling AnimateSlider(slider.value, oldMaxPoints,duration). oldMaxPoints refers to the olds level max points, so that the bar can fill up completely.

Then resets the value and maxvalue for the next level.
Calls AnimateSlider(0,points,duration) while points are now the points in the new level.

If no new level was reached, it directly just calls AnimateSlider(slider.value, points, duration).

Sets slider.value to points in the end.

### AnimateSlider(float from, float to, float duration)

Animates the slider value from "from" to "to" over time using linear interpolation (Mathf.Lerp).
Gradually updates the slider value over a fixed duration. And sets the final slider value exactly to the goal "to" after the animation completes.

## Data
/

## Notes
