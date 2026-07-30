# Webcommunication.cs

## Purpose

Handles the communication between the game and the server.

Sends the current player data to the server, checks the login data and makes sure that the current data is sent before the game is closed.

## Used By

StartUpManager and UI buttons or scripts which trigger sending or closing the game.

## Inspector References

| Field     | Type | Purpose                                                               |
| --------- | ---- | --------------------------------------------------------------------- |
| Send Data | bool | Debug value which triggers SendData() through Update() when activated |

## Classes

* LoginToken

  * private serializable container class for the login data
  * contains:

    * mode
    * player_id
    * password
* Webcommunication

## Important Methods

### SendData()

Checks if data is already being sent.

If no upload is active, it creates a new CommunicationToken and calls LoadData().

Converts the CommunicationToken into json and starts SendDataRoutine().

### SendDataRoutine(string json)

Checks if data is already being sent.

Sets isSending to true so that no second upload can start at the same time.

Creates a WWWForm and adds the json data with the field name:

jsondata

Creates a POST request to the server and sets a timeout of 10 seconds.

If the request fails, the error is printed for debugging.

If the request succeeds, the server response is printed and the returned binary data is stored in a byte array.

Disposes the request and sets isSending back to false.

### Update()

Checks if sendData is true and no upload or quitting process is currently active.

Calls SendData() and then sets sendData back to false.

This can be used to manually trigger an upload through the Inspector.

### CloseGameButton()

Gets called when the button for closing the game is clicked.

If the game is not already quitting, it starts SaveThenQuitRoutine().

### SaveThenQuitRoutine()

Sets isQuitting to true so that no additional upload or quitting process is started.

Waits until a currently active upload is finished.

Creates a new CommunicationToken, loads the current data and converts it into json.

Calls SendDataRoutine() and waits until the data was sent.

After the upload is finished, Application.Quit() is called.

### CheckLogin(string playerId, string password, Action<string> onFinished)

Creates a LoginToken with:

* mode set to login
* entered Player ID
* entered password

Converts the LoginToken into json and starts CheckLoginRoutine().

The provided onFinished Action is called after the server response was received.

### CheckLoginRoutine(string json, Action<string> onFinished)

Creates a WWWForm and adds the login json with the field name:

jsondata

Creates a POST request to the server and sets a timeout of 10 seconds.

If the request fails, it prints the error, HTTP response code and server response.

Then calls the onFinished Action with:

CONNECTION_ERROR

If the request succeeds, it removes spaces at the beginning and end of the server response and calls the onFinished Action with the received response.

Disposes the request after it is finished.

## Data

Sends data to the server as json inside a WWWForm field called jsondata.

For the normal data upload it sends the values stored inside CommunicationToken.

For the login check it sends:

* mode
* player ID
* password

The server response of the login request is passed back to the StartUpManager through an Action.

## Notes

The server address is stored inside the private server string.

The server request timeout is 10 seconds.

isSending prevents multiple normal data uploads from running at the same time.

isQuitting prevents new uploads from starting while the game is waiting to close.

The game only closes after the final data upload is completed.

SendDataRoutine() currently only prints upload errors and does not return a success or failure value.

CheckLoginRoutine() returns CONNECTION_ERROR for every failed web request.

The sendData bool is mainly used as a manual Debug trigger through the Inspector.

The TODO for checking the internet connection is not implemented yet.
