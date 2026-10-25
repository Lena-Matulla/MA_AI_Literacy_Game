using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.Rendering.STP;

/// <summary>
/// Handles the communication between the game and the server.
///Sends the current player data to the server, checks the login data 
///and makes sure that the current data is sent before the game is closed.
/// </summary>
public class Webcommunication : MonoBehaviour
{
    //Token for logincheck
    [System.Serializable]
    private class LoginToken
    {
        public string mode;
        public string player_id;
        public string password;
    }

    public bool sendData = false;
    private string server = "https://www.dh-profil.uni-tuebingen.de/kicher/kichercollector.php";
    private bool isSending = false;
    private bool isQuitting = false;

    //creates new token and calls the load function to load all current values into the token
    //Converts token to json and Starts the send coroutine
    public void SendData() {

        if (!isSending)
        {
            Debug.Log("KK: Started sending");
            CommunicationToken token = new CommunicationToken();
            token.LoadData();

            string json = JsonUtility.ToJson(token);

            StartCoroutine(SendDataRoutine(json));
        }
    }

    //Creates and handles post request to server
    IEnumerator SendDataRoutine(string json)
    {
        if (isSending)
        {
            yield break;
        }
        isSending = true;

        WWWForm form = new WWWForm();
        form.AddField("jsondata", json);

        UnityWebRequest www = UnityWebRequest.Post(server, form);
        www.timeout = 10;

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("KK RESULT");
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("KK RESULT");

            //Show results as text
            Debug.Log(www.downloadHandler.text);

            //Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }

        www.Dispose();
        isSending = false;
    }
    //used for manual triggering through inspector
    private void Update()
    {
        if (sendData && !isSending && !isQuitting)
        {
            SendData();
            sendData = false;
        }
    }

    //gets called when game is closed
    public void CloseGameButton()
    {
        if (!isQuitting)
        {
            StartCoroutine(SaveThenQuitRoutine());
        }

    }

    //first safe & then close the game
    private IEnumerator SaveThenQuitRoutine()
    {
        isQuitting = true;

        Debug.Log("Saving before quit...");

        //wait if there is a timer update already happening
        while (isSending)
        {
            yield return null;
        }

        CommunicationToken token = new CommunicationToken();
        token.LoadData();

        string json = JsonUtility.ToJson(token);

        yield return SendDataRoutine(json);

        Debug.Log("Save finished. Quitting game.");

        Application.Quit();
    }



    //Creates Login token & starts login coroutine
    public void CheckLogin(string playerId, string password, Action<string> onFinished)
    {
        LoginToken token = new LoginToken
        {
            mode = "login",
            player_id = playerId,
            password = password
        };

        string json = JsonUtility.ToJson(token);

        StartCoroutine(CheckLoginRoutine(json, onFinished));
    }

    //Creates and handles post request for login
    private IEnumerator CheckLoginRoutine(string json, Action<string> onFinished)
    {
        WWWForm form = new WWWForm();
        form.AddField("jsondata", json);

        UnityWebRequest www = UnityWebRequest.Post(server, form);
        www.timeout = 10;

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("Login check failed: " + www.error);
            Debug.LogWarning("HTTP Code: " + www.responseCode);
            Debug.LogWarning("Server answer: " + www.downloadHandler.text);

            onFinished?.Invoke("CONNECTION_ERROR");
        }
        else
        {
            string response = www.downloadHandler.text.Trim();
            Debug.Log("Login response: " + response);
            onFinished?.Invoke(response);
        }

        www.Dispose();
    }

}
