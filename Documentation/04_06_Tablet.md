# Tablet

## Purpose
Empty GameObject containing all parts regarding the Tablet. When opened the user can here get insight to the statistics of his gameplay.
On default disabled, gets enabled when it is clicked on the desk.

## Components
Background:
- Contains the the two design points
- one for the "camera" of the tablet
  
Home button.
  - This button can be clicked. with that the tablet is closed.

Text:
- Just a titel

Content:
- just there for debugging, can be ignored, is not used

Scroll View:
- scrollable gameobject (from Unity)
- Text:
  - Here all the important parts are listed.
  - Text has a Vertical Layout Group to sort all the different child text objects
  - different GameObjects as children to seperate the different statistic parts. (better overview)
  - There is also a Legend at the end to get information about the statistic.


## Responsibilities
Showcases the statistic

