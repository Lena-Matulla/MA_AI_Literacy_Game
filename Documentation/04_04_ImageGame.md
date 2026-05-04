# ImageGame

## Purpose
Empty GameObject containing all parts regarding the Image Decision sub-game.

## Components
Canvas:
- On default disabled, because it is only enabled once the laptop on the desk is clicked and gets disabled when home is clicked.
- Represents visually a computer screen
- Background is the desctop background image
- ProgramDesign
  - Contains the visual part of the "Program" which is open on the "Computer". Contains the different Buttons and slider. as well as the ImagePanel
    - ImagePanel:
      - Contains the RawImage which is set with a AspectRatioFitter to fit inside the parent
      - Marker which is disabled and gets enabled when the player is marking the point on the image he finds suspicious.
      - Interaction Layer with the MouseClickPosition script to mark the Image. See chapter 5 for more.
- Home Button, when clicked disable canvas to return to the half/half screen with the office and the desk
- TrustPointScore to display the current score since openening the Computer till it is closed again

Managers:

- ImageGameManager
  - Contains the TrialManager script in which the whole logic of the subgame happens. see chapter 5 for more.
- ExportManager
  - To export the files, but outdated!!!
  - Script: Exportmanager
- PasswordManager
  - To password safe the export
  - script: PasswordManager
  - also outdated

## Responsibilities
Contains all the sub parts of the Image Game and keeps them inside of one Group.

