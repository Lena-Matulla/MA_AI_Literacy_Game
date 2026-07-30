# TrialManager.cs

## Purpose

this script manages the trial flow/gameplay logic of the ImageGame.

It manages the selection between real and fake, the marking of fake media, the confidence rating, logging, scoring and loading the next media entry.

## Used By

ImageGameManager

## Inspector References

| Field                            | Type               | Purpose                                                                                       |
| -------------------------------- | ------------------ | --------------------------------------------------------------------------------------------- |
| On Correct Answer                | OnCorrectAnswer    | reference to the script which handles the correct answer animation and score display          |
| Image Provider                   | ImgProvider        | reference to the ImgProvider                                                                  |
| Data Logger                      | DataLogger         | reference to the DataLogger                                                                   |
| Interaction Layer                | MouseClickPosition | reference to the InteractionLayer                                                             |
| Real Button                      | Button             | reference to the real Button                                                                  |
| Fake Button                      | Button             | reference to the fake Button                                                                  |
| Confirme Button                  | Button             | reference to the confirme Button                                                              |
| Confirme Marked                  | Button             | reference to the confirme Marked button                                                       |
| Toggle                           | Toggle             | reference to the toggle                                                                       |
| Confidence Panel                 | GameObject         | Panel with the confidence rating                                                              |
| Confidence Slider                | Slider             | Confidence slider                                                                             |
| Confidence Button                | Button             | Ok button of the confidence                                                                   |
| Markerpart                       | GameObject         | reference to the Marker                                                                       |
| Why Input Field                  | TMP Input Field    | Input field where the user can explain why the media was selected as fake                     |
| Export Manager                   | ExportManager      | reference to the ExportManager. Currently not used because RegisterUsedImage is commented out |
| Normal Color                     | Color              | The normal color                                                                              |
| Selected Color                   | Color              | The color when it is selected                                                                 |
| Selected Scale                   | float              | Scalar for when it is selected (how much bigger it gets)                                      |
| Point Update                     | int                | How many points should be added when correct (default 10)                                     |
| Score This Round Text Field      | Text Mesh Pro UGUI | ref to Textfield where score of this round is safed (till computer is closed again)           |
| Number Of Image Til Notification | int                | How many media entries are shown until the reminder is activated                              |
| On Reminder Triggered            | UnityEvent         | Event which gets called when the reminder amount is reached                                   |

## Important Methods

### Start()

Adds Listener to the buttons and links the methods which are activated once something is detected.

Object and function it links to once changed/clicked:

* realButton => Select(Selection.Real)
* fakeButton => Select(Selection.Fake)
* confirmeButton => OnConfirmClicked()
* confienceButton => OnConfidenceOkClicked()
* confirmeMarked => OnConfirmMarkedClicked()
* toggle => OnToggleChanged(toggle)

Also deactivates the confidencePanel and calls ResetSelectionUI() and StartNewTrial().

### Select(Selection selection)

Sets the current selection and calls UpdateSelectionUI().

Counts how often the fake or real button is clicked during the current trial.

### ResetSelectionUI()

sets selectionstate to none and calls UpdateSelectionUI().

### UpdateSelectionUI()

if the current TrialState is Answering, then activate the confirmeButton if either Real or Fake is selected.

If the current TrialState is not Answering, the confirmeButton is not interactable.

Calls SetButtonVisual() for the Real and Fake buttons.

### SetButtonVisual(Button button, bool selected)

changes color and scale depending on what is selected.

### OnConfirmClicked()

Only works while the state is Answering.

Makes the Real, Fake and Confirme buttons not interactable anymore.

* If real is selected, it sets _ChoseFake to false, sets the local and normal values to the dummy values and calls EnterConfienceState().
* If fake is selected, it sets the state to Marking, sets _ChoseFake to true, automatically activates the toggle, activates the Markerpart and makes the Why Input Field interactable.

Because the toggle is automatically activated, the whole media is selected as fake by default.

### OnToggleChanged(Toggle toggle)

Activated if the toggle is interacted with.

Only works while Fake is selected and the current state is Marking. Otherwise the Interaction Layer is deactivated.

* If the toggle is on, the whole media is selected instead of a specific point. The previous marked point no longer counts, the local and normal values are changed to dummys and the Interaction Layer is deactivated.
* If the toggle is off, the user has to mark a new point and the Interaction Layer is activated.

If the current media is a video, the toggle is always forced on because videos cannot be marked with a specific point.

### OnConfirmMarkedClicked()

Called when Confirm Marked button is clicked.

Only works while Fake is selected and the current state is Marking.

If the Interaction Layer was interacted with or the toggle is activated, then EnterConfienceState() is called.

If the toggle is not on, it copies latestlocal and latestnormal into _Local and _Norm.

Also stores the text of the Why Input Field in whytext and clears the Input Field.

### EnterConfienceState()

state is set to Confidence.

All the UI interactables are locked or in case of the Interaction Layer deactivated.

The Why Input Field is also made not interactable.

The confidencePanel is then activated.

### OnConfidenceOkClicked()

Only works while the current state is Confidence.

Once the Confidence Ok Button is clicked, confidence is set to the value of the slider and the following is called:

FinalizeAndLog()

### FinalizeAndLog()

Gets the ground truth, filename and category from the ImgProvider.

Calculates the accuracy and the reactiontime in ms.

Logs all the important values by calling the DataLogger LogTrial function.

The logged values contain:

* trial index
* filename
* correct real or fake value
* selected real or fake value
* accuracy
* reaction time
* marked local and normal position
* if a point was marked
* confidence
* amount of Real button clicks
* amount of Fake button clicks
* text from the Why Input Field
* category
* media type

exportManager.RegisterUsedImage() is currently commented out.

If the answer is correct, it increases ScoreThisRound by pointUpdate and calls onCorrectAnswer.HandleCorrectAnswer() with the updated score. This handles the score display and animation.

Calls imageProvider.ConfirmCurrentImageCompleted() so that the current media is marked as completed in the study order.

Then calls ExitConfidenceState().

### ExitConfidenceState()

deactivates confidence panel and sets state to Answering.

Also reanables the Buttons of the UI.

Increases the imagecounter.

Calls StartNewTrial() as well as ResetSelectionUI().

### StartNewTrial()

Calls imageProvider.LoadNextStudyImage() to load the next media from the balanced study order.

If no media can be loaded, it calls EndStudy() and returns.

Increases the trial index and stores the new trial start time.

Resets all important parts back to default to start again with new media.

Resets:

* answered state
* marked state
* latest local and normal positions
* selected real or fake value
* saved local and normal positions
* confidence
* Real and Fake button click counters
* Markerpart
* toggle
* Interaction Layer

ScoreThisRound is not reset, because that is kept till the home button was activated and exportPointsOnClosed() gets called.

If the imagecounter reaches numberOfImageTilNotification, onReminderTriggered is called and the imagecounter is reset.

### updatemarked(Vector2 ll, Vector2 ln)

Gets called from the Interaction Layer when it is clicked.

Only works while Fake is selected and the state is Marking.

Sets marked to true and updates latestlocal and latestnormal.

Then calls UpdateSelectionUI().

### exportPointsOnClosed()

Called once the Home button is clicked.

Calls the ProgressManager AddPoints function with ScoreThisRound as variable. With that the score is added to the overall TrustValue score.

ScoreThisRound is then reset and ScoreThisRoundTextField is updated to show zero.

### EndStudy()

Gets called if ImgProvider cannot load another media entry.

Sets the state back to Answering and makes all buttons not interactable.

Deactivates:

* Interaction Layer
* Markerpart
* confidencePanel

Currently only calls a Debug message. Showing an end screen is still marked as TODO.

## Data

Stores the current selection, trial state, marked position, confidence, reaction time and number of button clicks.

Gets the current media information from the ImgProvider and sends all relevant trial values to the DataLogger.

Also manages the points earned during the current ImageGame session and the reminder counter.

## Notes

Two state types:

* Selection state

  * refers to if fake or real is clicked
  * { None, Real, Fake }
* Trial state

  * refers to the overall state the user is in
  * { Answering, Marking, Confidence }

Videos cannot be marked with a specific position. For videos the toggle is forced on and the whole video is selected as fake.

The method name EnterConfienceState() contains the spelling Confience instead of Confidence.

The ExportManager reference still exists, but RegisterUsedImage() is currently commented out.

The _hasAnsweredThisTrial and _toggleChecked variables are still present, but are not used by the active logging flow.
