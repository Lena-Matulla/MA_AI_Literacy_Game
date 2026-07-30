# Managers

# ProgressManager

## Purpose
Processes the progress of Points and Levels.

aka:
Point and Level System

## Components
- Script: ProgressManager
More in Chapter 5.
- AudioSource -> LevelUp sound

## Responsibilities
- Manages points and Levels

# SaveManager

## Purpose
Saves the current Progress in the persistent data path. Therefore the current progress is still there even if the game was closed.

## Components
Script: SaveManager
See chapter 5.

## Responsibilities
- saves progress in persistent Data path

# StatisticManager

## Purpose
Here the logic is handled to calculate statistics and showcase them on the tablet.

## Components
Scripts:
- CSVLogParser.cs
- Statistic.cs
- StatisticVisualize.cs

## Responsibilities

Calculate and showcase the statistic if needed.

# MusicManager

## Purpose
Handles the background music

## Components
Just AudioSource

## Responsibilities
Makes the backgroundmusic
Contains a mix of different musics

# TimeLimitManager

## Purpose
Handles the timelimit of the study. Warns when there are only 5 min left and stops the game once the time is up.

## Components
- Timer
  - OnTimeOutNotification:
    - TimeUpPopUp -> GameObjectSetActive true
    - TrialManager -> ExportPointsOnClosed
    - Canvas -> SetActive false
    - ClickHandler -> SetActive false
  - Timer2
    - 5MinLeftNotification -> SetActive true

## Responsibilities

Handle the time limit


# Bootstrap

## Purpose
Started before anything else, starts the tutorial and makes sure Config works correctly

## Components
Bootstrap.cs

## Responsibilities


# Webcommunicator

## Purpose
Handles the communication with the server

## Components
Scripts:
- Webcommunication.cs
- Timer
  - All 10 seconds send data

## Responsibilities


# OfficeProgressManager

## Purpose
  - Contains the UpdateOfficeVisual script
  - This script manages when what will be exposed in the office visually.
  - see more in chapter 5.

## Components

UpdateOfficeVisuals.cs