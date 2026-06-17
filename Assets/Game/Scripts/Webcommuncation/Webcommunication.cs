using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.Rendering.STP;

public class Webcommunication : MonoBehaviour
{
    public bool sendData = false;

    private string server = "https://www.dh-profil.uni-tuebingen.de/kicher/kichercollector.php";

    public void SendData() {
        Debug.Log("KK: Started sending");
        CommunicationToken token = new CommunicationToken();
        token.LoadData();
        
        string json = JsonUtility.ToJson(token);

        // Todo: Prüfen, ob Internetverbindung besteht
        StartCoroutine(SendDataRoutine(json));
    }

    IEnumerator SendDataRoutine(string json)
    {
        WWWForm form = new WWWForm();
        form.AddField("jsondata", json);

        UnityWebRequest www = UnityWebRequest.Post(server, form);


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
    }

    private void Update()
    {
        if (sendData)
        {
            SendData();
            sendData = false;
        }
    }

}
