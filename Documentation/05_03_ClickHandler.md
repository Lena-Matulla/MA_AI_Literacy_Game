# ClickHandler.cs

## Purpose
checks if the laptop, tablet or notebook on the desk is clicked. Enables the corresponding GameObject.

## Used By
ClickHandler

## Inspector References
| Field | Type | Purpose |
|---|---|---|
| Desk Cam | Camera | reference to the desk camera |
| Office Cam | Camera | reference to the office camera |
| Image Game Panel | Canvas | reference to the Canvas of the ImageGame to enable it |

## Important Methods

### Update()
Each frame it checks if the mouse clicked. if yes, it takes the current camera based on the screen position and checks if a collider with the tag "Laptop" was hit. If yes, it enables it.

Later on other objects (Tablet and Notebook) will be added

## Data


## Notes
!!!
Maybe change this or the other click handler -> adjust, because i changed the way i did it over time (both scripts were written with a lot of time inbetween)

