using UnityEngine;
using System;
using System.Data;
using System.IO;

public class SessionIDManager : MonoBehaviour
{

    public static string SessionID { get; private set; }
    public static DateTime sessionStartTime { get; private set; }

    public static void StartNewSession()
    {
        SessionID = Guid.NewGuid().ToString();
        sessionStartTime = DateTime.Now;
    }
}
