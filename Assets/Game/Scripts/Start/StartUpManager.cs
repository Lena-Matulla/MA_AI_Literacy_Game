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

    //Connection to the Webcommunicator for the Login Check
    [SerializeField] private Webcommunication webcommunication;



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

        /*
        GameConfigManager.SetPlayerID(enteredId);
        GameConfigManager.SetPassword(enteredPassword);

        SessionIDManager.StartNewSession();

        Debug.Log($"Player: {GameConfigManager.Config.playerID}, " +
                  $"Experiment: {GameConfigManager.Config.experimentID}, " +
                  $"Server: {GameConfigManager.Config.serverURL}");

        SceneManager.LoadScene(gameplaySceneName);
        */

        webcommunication.CheckLogin(enteredId, enteredPassword, HandleLoginResponse);

    }

    //gets called once the webcommunicator finished his work, (action)
    private void HandleLoginResponse(string response)
    {
        string enteredId = playerInput.text.Trim();
        string enteredPassword = passwordInput.text.Trim();

        if (response == "LOGIN_OK" || response == "NEW_PLAYER")
        {
            GameConfigManager.SetPlayerID(enteredId);
            GameConfigManager.SetPassword(enteredPassword);

            SessionIDManager.StartNewSession();

            Debug.Log($"Player: {GameConfigManager.Config.playerID}, " +
                      $"Experiment: {GameConfigManager.Config.experimentID}, " +
                      $"Server: {GameConfigManager.Config.serverURL}");

            SceneManager.LoadScene(gameplaySceneName);
        }
        else if (response == "WRONG_PASSWORD")
        {
            ShowError("Wrong password.");
            Debug.LogWarning("Wrong password entered.");
        }
        else if (response == "CONNECTION_ERROR")
        {
            ShowError("Could not connect to server.");
        }
        else if (response == "PLAYER_DOES_NOT_EXIST")
        {
            ShowError("This Player ID does not exist.");
        }
        else
        {
            ShowError("Login failed. Please contact admin.");
            Debug.LogWarning("Unknown login response: " + response);
        }
    }

    private void ShowError(string message)
    {
        if (errorText != null)
        {
            errorText.text = message;
        }
    }




}

