using UnityEngine;
using System;
using System.Data;
using System.IO;

/// <summary>
/// Class to handle the SessionID 
/// (New each time the game is closed and started again (not the ingame game, but the whole))
/// </summary>
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
