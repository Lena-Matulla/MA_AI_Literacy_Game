# StartUpManager.cs

## Purpose

Manages the login at the start of the game.

Checks the entered Player ID and password through the Webcommunication class. If the login is successful, it saves the login data, starts a new session and loads the gameplay scene.

## Used By

StartUp scene and the Login UI.

## Inspector References

| Field               | Type             | Purpose                                                                         |
| ------------------- | ---------------- | ------------------------------------------------------------------------------- |
| Player Input        | TMP Input Field  | Input field where the Player ID is entered                                      |
| Password Input      | TMP Input Field  | Input field where the password is entered                                       |
| Error Text          | TMP Text         | Textfield where login and connection errors are displayed                       |
| Gameplay Scene Name | string           | Name of the scene which is loaded after a successful login                      |
| Required Password   | string           | Password for the old local login check. Currently not used                      |
| Webcommunication    | Webcommunication | reference to the Webcommunication script which checks the login with the server |

## Important Methods

### Start()

Calls GameConfigManager.LoadOrCreateConfig().

Clears the error text if the reference exists.

If a Player ID was already stored in the GameConfig, it fills the Player Input Field with the saved Player ID.

Clears the Password Input Field and sets its content type to Password so the entered text is hidden.

### ConfirmPlayerID()

Gets the entered Player ID and password from the Input Fields and removes spaces at the beginning and end.

Checks if the Player ID is empty.

If it is empty, an error message is shown and the method returns.

Checks if the password is empty.

If it is empty, an error message is shown and the method returns.

Calls webcommunication.CheckLogin() with:

* entered Player ID
* entered password
* HandleLoginResponse() as callback

The old local password check and direct scene loading are currently commented out.

### HandleLoginResponse(string response)

Gets called once the Webcommunication script finished the login check.

Gets the current Player ID and password from the Input Fields.

Handles the different server responses.

If the response is LOGIN_OK or NEW_PLAYER:

* saves the Player ID in the GameConfig
* saves the password in the GameConfig
* starts a new Session ID
* prints the Player ID, Experiment ID and Server URL for debugging
* loads the gameplay scene

If the response is WRONG_PASSWORD, it shows that the password is wrong.

If the response is CONNECTION_ERROR, it shows that the connection to the server failed.

If the response is PLAYER_DOES_NOT_EXIST, it shows that the entered Player ID does not exist.

For every other response, it shows a general login error and prints the unknown response as a Debug warning.

### ShowError(string message)

Sets the Error Text to the provided message if the Error Text reference exists.

## Data

Gets the Player ID and password from the Login UI.

Stores a successful Player ID and password through the GameConfigManager.

Starts a new session through the SessionIDManager after a successful login.

## Notes

The Webcommunication reference has to be assigned in the Inspector.

The gameplay scene has to exist and its name has to match gameplaySceneName.

The requiredPassword variable is currently not used because the local password check is commented out.

The login response strings have to match the response values returned by the server.

A new session is only started after a successful login.
