# Office Block

## Purpose
Empty parent to store all parts regarding the office (top half of the screen).
The top part is not really playable by the player. It is a visualization of the players choices and a gamification to make it more interesting.

## Components
- Office Camera

In OfficeRoot:
- Background_Tile
  - Includes all the tiles which are part to the background.
- Decorations
  - include all the visual decorations which are added to the office over time.
  - they are each sorted by room and then in there also sorted by level when they are exposed
- NPCs
  - includes all the NPCs and their target points. Those are the points where the NPCs are walking to.
- Overlays
  - The black overlays which hide the rooms before they are exposed in their regarding levels


#
- A*
  - The package which is used for the NPCs logic. It works by an pathfinding algorithm.
  - Ref: https://arongranberg.com/astar/docs/

## Responsibilities
- takes care of the whole office
- manages all of the Parts regarding the office


