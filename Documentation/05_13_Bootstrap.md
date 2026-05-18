# Bootstrap.cs

## Purpose
Called when the game is started. Manages that the Config and the SessionID is called in the beginning.

## Used By
Bootstrap

## Inspector References
/

## Important Methods

### Awake()
Called when the game is started.
Calls:
- GameConfigManager.LoadOrCreateConfig()
- SessionIDManager.StartNewSession();

then prints out the current playerID, experimentID and Server URL for debugging.

## Data
/

## Notes


