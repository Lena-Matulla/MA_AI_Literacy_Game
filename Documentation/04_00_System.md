# System

The Game is divided in different Groups through empty parents and blocks in the Hierarchy.

- UI Canvas, which includes the Canvas which spans over the whole screen all the time, it provides:
  -  the Level and Trustvalue Bar.
  -  QuitButton to end the Game
  -  Tutorial Button to reopen the tutorial
  -  The Tutorial itself
  -  The Notifications regarding:
     -  5 min playtime left
     -  playtime up (locks the game)
     -  reminder to not always stay in the computer screen
- The Office block containing the Camera, the OfficeRoot and the A* (A* Pathfinding plugin/package for the npcs to walk around)
- The Desk block containing the Camrea as well as the Desk with the clickable items
- The different Parts of the Game:
  - The ImageGame, which contains all features needed for the Image decision part of the Game
  - The Tablet contains the UI for the Tablet when opened. There the statistic is displayed for insight to the player.
  - The Notebook containing different tipps and tricks which get unlocked with new levels
- More Managers:
  -  ProgressManager
  -  SaveManager
  -  StatisticManager
  -  Bootstrap
  -  Webcommunicator
  -  MusicManager
  -  TimeLimitManager
  -  OfficeProgressManager

All of them will be explained further in the next files.