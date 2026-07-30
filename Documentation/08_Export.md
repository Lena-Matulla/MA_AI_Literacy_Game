# Export

The old manual export button and zip file export are currently not used.

The data is now automatically sent to the server through the Webcommunication script.

The upload contains:

* Player ID
* Password
* complete csv-log file of the player
* complete config file
* current TrustValue points
* current level
* amount of ImageGame playthroughs

The data is stored inside a CommunicationToken, converted into json and sent to the server.

When the Close Game button is clicked, the game first waits for an already active upload to finish.

After that, the current data is loaded and sent to the server. The game only closes after the final upload is finished.

A data upload can also be triggered through SendData(). The sendData bool can be used as a manual Debug trigger through the Inspector.

The old zip file containing the csv-log, used images and session information is no longer created.
