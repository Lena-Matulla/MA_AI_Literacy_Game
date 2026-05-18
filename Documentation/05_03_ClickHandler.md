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
Each frame it checks if the mouse clicked. if yes, it selects either deskCam or officeCam depending on whether the mouse is in the lower or upper half of the screen. It then checks if a collider with the tag "Laptop" was hit. If yes, it enables it.

When the Tablet is clicked it calls the LoadData() of Statistic. Each time it is clicked the current data is read out to calculate and show current statistics. That is handled trough the statistic script. Can be seen in "05_15_Statistic.md"

Later on other objects (Notebook) will be added

## Data


## Notes
!!!
Maybe change this or the other click handler -> adjust, because i changed the way i did it over time (both scripts were written with a lot of time inbetween)

