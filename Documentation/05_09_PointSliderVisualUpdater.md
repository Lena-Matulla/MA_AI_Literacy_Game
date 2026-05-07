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
Updates the maxpoints value of the slider and starts the animation coroutine by calling StartCoroutine(AnimateBar(points)). If already a coroutine is running, it stops that.

### AnimateBar(int points)
Animates the slider value over time using linear interpolation (Mathf.Lerp).
Stores the current slider value as the animation start point. Then
gradually updates the slider value over a fixed duration (1.5f seconds). and sets the final slider value exactly to points after the animation completes.

## Data
/

## Notes
