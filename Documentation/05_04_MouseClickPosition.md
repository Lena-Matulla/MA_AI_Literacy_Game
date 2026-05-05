# MouseClickPosition.cs


## Purpose
when enabled, checks wheather the image is clicked and where. Marks that position with a red dot and safes the location in local and normalized coordinates. Then invokes the OnImageClicked Unityevent. Here the Trialmanager is attached in the inspector.updatemarked to update the click positions.

## Used By
InteractionLayer

## Inspector References
| Field | Type | Purpose |
|---|---|---|
| Canvas | Canvas | The Canvas in which it is clicked |
| Image Rect | Rect Transform | the rectangular in which it is clicked |
| marker | Gameobject | reference to the marker point Gameobject |
| LastLocal | Vector2 | to store the local coordinates |
| LastNormal | Vector2 | to store the normal coordinates (0-1 range)|
| interacted | bool | to check if an interaction happened since the last reset |

## Important Methods

### OnDisable()
reset all variables

### Reset()
get imageRect and canvas

### OnPointerClick(PointerEventData eventData)
if pointerclick is detected, change screen coordinates to in rect coordinates.
if the point is inside, calculate and update lastLocal and lastNormal.

### OnDisable()
diables the marker object

## Data


## Notes
!!!
Maybe change this or the other click handler -> adjust, because i changed the way i did it over time (both scripts were written with a lot of time inbetween)
