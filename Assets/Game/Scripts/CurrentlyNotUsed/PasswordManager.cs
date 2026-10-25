using UnityEngine;
using UnityEngine.UI;

public class PasswordManager : MonoBehaviour
{
    [SerializeField] private string password;
    [SerializeField] private Button downloadButton;
    [SerializeField] GameObject tryAgainTextField;

    public void testPassword(string tocheckpassword)
    {
        if(tocheckpassword == password)
        {
            Debug.Log("correct password");
            tryAgainTextField.SetActive(false);
            downloadButton.interactable = true;
        }
        else
        {
            Debug.Log("false password");
            tryAgainTextField.SetActive(true);
        }
    }

    public void ResetState()
    {
        downloadButton.interactable=false;
        tryAgainTextField.SetActive(false);
    }
}
