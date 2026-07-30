# Data

The game saves one row per trial in a csv.

All sessions of the same player are stored in the same player-specific csv file.

## What is logged:

| name in header      | variable name           | use                                                                                    |
| ------------------- | ----------------------- | -------------------------------------------------------------------------------------- |
| player_id           | PlayerId                | ID of the current player                                                               |
| session_id          | SessionId               | ID of the current play session                                                         |
| session_start_date  | _sessionStartTimeString | start date and time of the session                                                     |
| session_duration_ms | sessionDurationMs       | duration of the session in ms until the trial was logged                               |
| trial_index         | trialIndex              | indication of the trial number -> how many media entries were shown already            |
| image_name          | imageName               | filename of the current image or video                                                 |
| ground_truth        | groundTruth             | correct answer: real or fake                                                           |
| user_choice         | userChoice              | user decision on real or fake                                                          |
| accuracy            | accuracy                | 0 = incorrect, 1 = correct                                                             |
| reaction_time_ms    | reactionTimeMs          | time in ms for this trial                                                              |
| lastlocalx          | lastLocal.x             | marked X position in local image coordinates                                           |
| lastlocaly          | lastLocal.y             | marked Y position in local image coordinates                                           |
| lastnormalx         | lastNormal.x            | normalized marked X position from 0 to 1                                               |
| lastnormaly         | lastNormal.y            | normalized marked Y position from 0 to 1                                               |
| imageMarked         | imageMarked             | stores if the user marked a specific point                                             |
| confidence          | confidence              | specified confidence value from the confidence slider                                  |
| RealclickedInTrial  | realclicked             | how often the Real button was clicked in this trial                                    |
| FakeClickedInTrial  | fakeclicked             | how often the Fake button was clicked in this trial                                    |
| whyText             | whyText                 | input from the Input Field where the user can explain why they think the media is fake |
| currentCategory     | currentCategory         | category of the current media                                                          |
| mediaType           | mediaType               | defines if the current media is an Image or Video                                      |

## Notes

If the whole image or video is selected instead of a specific point, dummy position values are stored.

The local dummy position is:

* X: -1000
* Y: -1000

The normalized dummy position is:

* X: -1
* Y: -1

Videos cannot be marked at a specific point and therefore use the whole-media selection.

Float values are stored with a dot as decimal separator.
