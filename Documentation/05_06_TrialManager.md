# TrialManager.cs

## Purpose
Complete logic behind the Imagegame

## Used By
ImageGameManager

## Inspector References
| Field | Type | Purpose |
|---|---|---|
| Image Provider | ImageProvider | reference to the ImageProvider |
| Data Logger | DataLogger | reference to the DataLogger |
| Interaction Layer | InteracttionLayer | reference to the InteractionLayer |
| Real Button | Button | reference to the real Button |
| Fake Button | Button | reference to the fake Button |
| Confirme Button | Button | reference to the confirme Button |
| Confirme Marked | Button | reference to the confirme Marked button |
| Toggle | Toggle | reference to the toggle |
Confidence Panel | GameObject | Panel with the confidence rating |
Confidence Slider | Slider | Confidence slider |
Confidence Button | Button | Ok button of the confidence |
Markerpart | GameObject | reference to the Marker |
Export Manager | ExportManager | reference to the ExportManager |
Normal Color | color | The normal color |
Selected Color | color | The color when it is selected |
Selected Scale | float | Scalar for when it is selected (how much bigger it gets)|
Point Update | int | How many points should be added when correct (default 10)|
Score This Round Text Field | Text Mesh Pro UGUI | ref to Textfield where score of this round is safed (till computer is closed again)|

## Important Methods

### Start()
Adds Listener to the buttons and links the methods which are activated once something is detected.

Object and function it links to once changed/clicked:

- realButton => Select(Selection.Real)
- fakeButton => Select(Selection.Fake)
- confirmeButton => OnConfirmClicked()
- confienceButton => OnConfidenceOkClicked()
- confirmeMarked => OnConfirmMarkedClicked()
- toggle => OnToggleChanged(toggle)

Also calls ResetSelectionUI() and StartNewTrial()

### Selection(Selection selection)
Calls UpdateSelectionUI()
Counts how often fake/real button is clicked

### ResetSelectionUI()
sets selectionstate to none and calls UpdateSelectionUI()

### UpdateSelectionUI()
if the current TrialState is Answering, then activate the confirmeButton if either Real or Fake is selected

Calls SetButtonVisual

### SetButtonVisual(Button button, bool selected)
changes color and scale depending on what is selected

### OnConfirmClicked()
makes the buttons not interactable anymore.
- If real is selected, it sets the local and normal value to the dummy values and calls EnterConfidenceState()
- If fake is selected, it starts the state = Marking and activates the interactionLayer as well as the Makerpart.

### OnToggleChanged(Toggle toggle)
Activated if the toggle is interacted with.
- If it is on, the local and norm values are changed to dummys, because the overall image is chosen as indicator for fake rather than a specific point and the interactionLayer is deactivated so it is not markable anymore
- if it is off, then the interactionLayer is activated again

### OnConfirmMarkedClicked()
Called when Confirm Marked button is clicked.
If the interactionlayer is interacted with or the toggle is activated, then the EnterConfidenceState() is called

### EnterConfidenceState()
state is set to Confidence.
All the UI interactables are locked or in case of the interactionLayer deactivated.
The confidencePanel is then activated.

### OnConfidenceOkClicked()
Once the Confidence Ok Button is clicked, this gets called.
confidence is set to the value of the slieder and the following is called:
FinalizeAndLog()

### FinalizeAndLog()
calculates the accuracy and the reactiontime in ms.
logs all the important values by calling the DataLogger LogTrial function.

Calls exportManager.RegisterUsedImage(imageProvider.CurrentImageEntry) to safe the images 
(maybe not needed in future)

if correct answer, ScoreThisRound and its Textfield

Then calls ExitConfidenceState()

### ExitConfidenceState()
deactivates confidence panel and sets state to Answering.
also reanables the Buttons of the UI
Calls StartNewTrial() as well as ResetSelectionUI()

### StartNewTrial()
Resets all important parts back to default to start again with a new image. (ScoreThisRound is not reset, because that is kept till the home button was activated and exportPointsOnClosed() gets called)

### updatemarked(Vector2 ll, Vector2 ln)
gets called from the interactionLayer when it is clicked. The local and normal values are then updated here 

### exportPointsOnClosed()
Called once the Home button is clicked. Calls the ProgressManager AddPoints function with ScoreThisRound as variable. With that the score is added to the overall TrustValue score. 
The ScoreThisRound is then reset.

## Data
see above

## Notes
Two state types:
- Selection state 
  - refers to if fake or true is clicked 
  - { None, Real, Fake}
- Trial state
  - refers to the overall state the user is in ()
  - { Answering, Marking,Confidence}

