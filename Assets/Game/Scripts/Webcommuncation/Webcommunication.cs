using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.Rendering.STP;


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

    public void SendData() {

        if (!isSending)
        {
            Debug.Log("KK: Started sending");
            CommunicationToken token = new CommunicationToken();
            token.LoadData();

            string json = JsonUtility.ToJson(token);

            // Todo: Prüfen, ob Internetverbindung besteht
            StartCoroutine(SendDataRoutine(json));
        }
    }

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

            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }

        www.Dispose();
        isSending = false;
    }

    private void Update()
    {
        if (sendData && !isSending && !isQuitting)
        {
            SendData();
            sendData = false;
        }
    }

    public void CloseGameButton()
    {
        if (!isQuitting)
        {
            StartCoroutine(SaveThenQuitRoutine());
        }

    }

    private IEnumerator SaveThenQuitRoutine()
    {
        isQuitting = true;

        Debug.Log("Saving before quit...");

        //Falls gerade ein normaler Timer-Upload läuft: warten
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



    //Login:

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
