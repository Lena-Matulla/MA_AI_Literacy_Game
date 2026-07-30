# TutorialManager.cs

## Purpose

Manages the different pages of the Tutorial.

Handles the Next and Back buttons and activates the correct Tutorial page.

Also deactivates the click GameObject while the Tutorial is open.

## Used By

Tutorial UI

## Inspector References

| Field        | Type         | Purpose                                                                                          |
| ------------ | ------------ | ------------------------------------------------------------------------------------------------ |
| Game Objects | GameObject[] | array which contains all Tutorial pages                                                          |
| Current Pos  | int          | current position inside the Tutorial page array                                                  |
| Next         | Button       | reference to the Next button                                                                     |
| Back         | Button       | reference to the Back button                                                                     |
| Click        | GameObject   | GameObject which is deactivated while the Tutorial is open and activated again when it is closed |

## Important Methods

### OnEnable()

Called when the TutorialManager GameObject is activated.

Deactivates the click GameObject.

### OnDisable()

Called when the TutorialManager GameObject is deactivated.

Activates the click GameObject again if the reference is not null.

### NextButton()

Called when the Next button is clicked.

Checks if the current page is not the final page.

Increases currentPos and calls OpenPage().

### BackButton()

Called when the Back button is clicked.

Checks if the current page is not the first page.

Decreases currentPos and calls OpenPage().

### OpenPage()

Loops through all GameObjects inside the gameObjects array.

Activates the GameObject at currentPos and deactivates all other Tutorial pages.

Updates the interaction state of the Next and Back buttons.

* On the first page, the Back button is not interactable.
* On the final page, the Next button is not interactable.
* On every page between the first and final page, both buttons are interactable.

## Data

Stores the Tutorial pages inside a GameObject array.

Uses currentPos to store which Tutorial page is currently open.

## Notes

The order of the Tutorial pages is based on their position inside the gameObjects array.

currentPos starts at zero, which represents the first Tutorial page.

OpenPage() is only called after the Next or Back button is clicked. It is not automatically called inside OnEnable().

The first Tutorial page and the initial button states therefore have to be set correctly in the Inspector or by another script.

The script assumes that gameObjects contains at least one Tutorial page.
