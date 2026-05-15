using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    private void Awake()
    {
        GameConfigManager.LoadOrCreateConfig();
        SessionIDManager.StartNewSession();

        string playerId = GameConfigManager.Config.playerID;
        string experimentId = GameConfigManager.Config.experimentID;
        string serverUrl = GameConfigManager.Config.serverURL;

        Debug.Log($"Player: {playerId}, Experiment: {experimentId}, Server: {serverUrl}");
    }
}
