using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "progress.json");

    public static void Save(GameProgressData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("Saved progress to: " + SavePath);
    }

    public static GameProgressData Load()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("No save file found. Creating new");
            return new GameProgressData();
        }

        string json = File.ReadAllText(SavePath);
        GameProgressData data = JsonUtility.FromJson<GameProgressData>(json);
        Debug.Log("Loaded Progress from: " + SavePath);
        return data;
    }

    public static void DeleteSave()
    {
        if (!File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("Save deleted");
        }
    }


}
