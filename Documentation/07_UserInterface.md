## Interface

The Game is divided into a top and bottom part.

The top half shows a top-down view of a 2D office. This part grows over time depending on the correct answers of the player. The higher the "TrustValue", the bigger the office expands and the more objects and NPCs are there.

The bottom half of the screen displays a desk with objects on it.

The objects are:

* Laptop
* Tablet
* Notebook

On the top left of the screen is a Pointbar as well as a Level indicator. Those represent the TrustValue score and the regarding current Level of the Office.

### Laptop

When clicking on the Laptop, a canvas which spans over more than 2/3 of the whole screen is opened. This mimics a computer screen. On the screen the ImageGame is opened.

#### ImageGame

The ImageGame is the central game piece of this Game. Here images and videos are displayed, and the user goes through different steps on an interface:

* Decision about real and fake

  * Here the player decides if they think the displayed media is real or fake and then clicks okay.
* Mark

  * If the player clicked fake, the marked part is visible.
  * Here the player either clicks on the image to mark the position which indicated that the image is fake or checks "overall" to indicate that the overall feeling of the image made them suspicious.
  * Videos cannot be marked at a specific position. Therefore "overall" is automatically selected for videos.
  * The player can also enter a text which explains why they think the media is fake.
* Confidence

  * The confidence part is visible either after deciding real or after confirming the marking in case of a fake answer.
  * Here the player rates their confidence in the decision over a slider and then confirms it.

#### Rest of the screen

On the rest of the visual computer screen the current score of this round is saved.

Once the screen is closed through the Home button, those points will be added to the overall TrustValue score.

### Tablet
The tablet showcases the statistical insight of the answers of the player


### Notebook
The notebook shows part of the story. Here are also new tips and tricks added with each level up