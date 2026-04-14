using UnityEngine;
using System;
using System.IO;

public class DataLogger : MonoBehaviour
{
    public string SessionId { get; private set; }
    public string FilePath { get; private set; }

    private bool _headerWritten = false;
    private DateTime _sessionStartTime;
    private string _sessionStartTimeString;


    private void Awake()
    {
        SessionId = Guid.NewGuid().ToString("N");
        _sessionStartTime = DateTime.Now;
        _sessionStartTimeString = _sessionStartTime.ToString("yyyy-MM-dd HH:mm:ss");

        string logsDir = Path.Combine(Application.persistentDataPath, "Logs");
        Directory.CreateDirectory(logsDir);

        FilePath = Path.Combine(logsDir, $"session_{SessionId}_trials.csv");

        WriteHeaderIfNeeded();
        Debug.Log($"Logging to: {FilePath}");
    }

    private void WriteHeaderIfNeeded()
    {
        if (_headerWritten) return;
        if(!File.Exists(FilePath) || new FileInfo(FilePath).Length == 0 )
        {
            string header = "session_id,session_start_date,session_duration_ms,trial_index,image_name,ground_truth,user_choice,accuracy,reaction_time_ms, lastlocal, lastnormal, confidence, RealclickedInTrial, FakeClickedInTrial\n";
            File.AppendAllText(FilePath, header);
        }
        _headerWritten = true;
    }

    public void LogTrial(
        int trialIndex,
        string imageName,
        bool groundTruthIsFake,
        bool userChoseFake,
        int accuracy,
        int reactionTimeMs,
        Vector2 lastLocal,
        Vector2 lastNormal,
        float confidence,
        int realclicked,
        int fakeclicked) 
    {
        WriteHeaderIfNeeded();

        string groundTruth = groundTruthIsFake ? "fake" : "real";
        string userChoice = userChoseFake ? "fake" : "real";

        int sessionDurationMs = (int)(DateTime.Now - _sessionStartTime).TotalMilliseconds;

        string line = $"{ SessionId }, {EscapeCsv(_sessionStartTimeString)},{sessionDurationMs},{ trialIndex},{ EscapeCsv(imageName)},{ groundTruth},{ userChoice},{ accuracy},{ reactionTimeMs},{lastLocal},{lastNormal},{confidence},{realclicked},{fakeclicked}\n";
        File.AppendAllText(FilePath, line );
    }

    private string EscapeCsv(string value)
    {
        if (string.IsNullOrEmpty(value)) return "";
        bool mustQuote = value.Contains(",") || value.Contains("\"") || value.Contains("\n") || value.Contains("\r");
        if (!mustQuote) return value;

        string escaped = value.Replace("\"", "\"\"");
        return $"\"{escaped}\"";
    }

}
