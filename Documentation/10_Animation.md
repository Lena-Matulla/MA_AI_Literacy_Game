# Animation-PopUp

Different PopUp animations are included to make the game more enjoyable and to give back more feedback to the user.

The different PopUps:
- CorrectAnswer
- LevelUp


Where do i find them (if i want to change them):
- CorrectAnswer:
  - in ImageGame/Canvas
- LevelUp
  - Notebook

How do they work:
They have a manager, through which the animation can be started by being called from some other script:

- CorrectAnser: throuth the "OnCorrectAnswer" Script which is attached to the ImageGameManager
- LevelUp: from the Progressmanager

The animations then start the sound and also deactivate the gameobject automatically after finishing the animation.