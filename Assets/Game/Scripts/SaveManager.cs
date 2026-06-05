using UnityEngine;
using System.IO;

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