using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class StartUpManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerInput;
    [SerializeField] private TMP_Text errorText;
    [SerializeField] private string gameplaySceneName = "GameScene";

    private void Start()
    {
        GameConfigManager.LoadOrCreateConfig();

        if (errorText != null)
        {
            errorText.text = "";
        }

        if (!string.IsNullOrEmpty(GameConfigManager.Config.playerID))
        {
            playerInput.text = GameConfigManager.Config.playerID;
        }
    }

    public void ConfirmPlayerID()
    {
        string enteredId = playerInput.text.Trim();

        if (string.IsNullOrWhiteSpace(enteredId))
        {
            if (errorText != null)
            {
                errorText.text = "Please enter a player ID.";
            }

            Debug.LogWarning("Player ID cannot be empty.");
            return;
        }

        GameConfigManager.SetPlayerID(enteredId);

        SessionIDManager.StartNewSession();

        Debug.Log($"Player: {GameConfigManager.Config.playerID}, " +
                  $"Experiment: {GameConfigManager.Config.experimentID}, " +
                  $"Server: {GameConfigManager.Config.serverURL}");

        SceneManager.LoadScene(gameplaySceneName);


    }




}

