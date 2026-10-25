using UnityEngine;
using System.IO;

/// <summary>
///Saves the current Progress in the persistent data path. Therefore the current progress is still there even if the game was closed.
///Creates a separate save file for every player based on the player ID from the GameConfigManager.
///data is given and stored as GameProgressData object and converted to json
/// </summary>

public class SaveManager : MonoBehaviour
{
    private static string SaveDirectory =>
        Path.Combine(Application.persistentDataPath, "Saves");

    private static string GetSavePath()
    {
        if (GameConfigManager.Config == null)
        {
            Debug.LogError("Cannot get save path because GameConfigManager.Config is null.");
            return null;
        }

        string playerId = GameConfigManager.Config.playerID;

        if (string.IsNullOrWhiteSpace(playerId))
        {
            Debug.LogError("Cannot get save path because player ID is missing.");
            return null;
        }

        string safePlayerId = MakeFileNameSafe(playerId);

        return Path.Combine(SaveDirectory, $"player_{safePlayerId}_progress.json");
    }

    //save the data
    public static void Save(GameProgressData data)
    {
        string path = GetSavePath();

        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        Directory.CreateDirectory(SaveDirectory);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);

        Debug.Log("Saved progress to: " + path);
    }

    //load current data
    public static GameProgressData Load()
    {
        string path = GetSavePath();

        if (string.IsNullOrEmpty(path))
        {
            return new GameProgressData();
        }

        if (!File.Exists(path))
        {
            Debug.Log("No save file found for this player. Creating new progress.");
            return new GameProgressData();
        }

        string json = File.ReadAllText(path);
        GameProgressData data = JsonUtility.FromJson<GameProgressData>(json);

        Debug.Log("Loaded progress from: " + path);

        return data;
    }

    public static void DeleteSave()
    {
        string path = GetSavePath();

        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save deleted: " + path);
        }
    }

    private static string MakeFileNameSafe(string input)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            input = input.Replace(c, '_');
        }

        return input;
    }
}