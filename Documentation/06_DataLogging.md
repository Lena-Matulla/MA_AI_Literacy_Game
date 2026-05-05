# Data

The game saves one row per trial in a csv.

## What is logged:

| name in header | variable name | use |

- session_id | SessionId | Id of the current play session
- sessionStartTime | _sessionStartTimeString | start time of the session itself
- session_duration_ms | sessionDurationMs | duration of the session in ms
- trial_index | trialIndex | indication of the trial number -> how many images where shown already
- image_name | imageName | Name of the current image
- ground_truth | groundTruth | real or fake
- user_choice | userChoice | user decision on real or fake
- accuracy | accuracy| 0 = incorrect, 1 = correct.
- reaction_time_ms | reactionTimeMs | time in ms for this trial
- lastlocal | lastLocal | image click/mark position in image coordinates
- lastnormal | lastNormal | image click/mark position in percent normalized (from 0,0 (left down) to 1,1 (right up))
- OverallToggleChecked | toggleChecked | user selected the whole image as suspicious instead of marking a specific point.
- confidence | confidence | specified confidence value from 1-10
- RealclickedInTrial | realclicked | How often was the "Real" button clicked in this Trial
- FakeClickedInTrial | fakeclicked | How often was the "Fake" button clicked in this Trial