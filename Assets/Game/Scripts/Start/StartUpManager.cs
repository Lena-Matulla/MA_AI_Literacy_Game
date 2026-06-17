using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class StartUpManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_Text errorText;
    [SerializeField] private string gameplaySceneName = "GameScene";

    [SerializeField] private string requiredPassword = "test123";

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

        if (passwordInput != null)
        {
            passwordInput.text = "";
            passwordInput.contentType = TMP_InputField.ContentType.Password;
        }
    }

    public void ConfirmPlayerID()
    {
        string enteredId = playerInput.text.Trim();
        string enteredPassword = passwordInput.text.Trim();

        if (string.IsNullOrWhiteSpace(enteredId))
        {
            if (errorText != null)
            {
                errorText.text = "Please enter a player ID.";
            }

            Debug.LogWarning("Player ID cannot be empty.");
            return;
        }

        if (string.IsNullOrWhiteSpace(enteredPassword))
        {
            if (errorText != null)
            {
                errorText.text = "Please enter a password.";
            }
            Debug.LogWarning("Password cannot be empty.");
            return;
        }

        //TODO
        /*
        if (enteredPassword != requiredPassword)
        {
            ShowError("Wrong password.");
            Debug.LogWarning("Wrong password entered.");
            return;
        }
        */


        GameConfigManager.SetPlayerID(enteredId);
        GameConfigManager.SetPassword(enteredPassword);

        SessionIDManager.StartNewSession();

        Debug.Log($"Player: {GameConfigManager.Config.playerID}, " +
                  $"Experiment: {GameConfigManager.Config.experimentID}, " +
                  $"Server: {GameConfigManager.Config.serverURL}");

        SceneManager.LoadScene(gameplaySceneName);


    }




}

