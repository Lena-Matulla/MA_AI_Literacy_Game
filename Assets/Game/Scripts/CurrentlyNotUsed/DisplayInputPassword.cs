using UnityEngine;
using TMPro;

public class DisplayInputPassword : MonoBehaviour
{
    [Header("Value from input field")]
    [SerializeField] private string inputText;

    [Header("Displayed Text")]
    [SerializeField] private GameObject reactionGroup;
    [SerializeField] private TMP_Text displayedText;

    public void GrabFromInputField(string input)
    {
        inputText = input;
        DisplayReaction();
    }

    public void DisplayReaction()
    {
        displayedText.text = inputText;
        reactionGroup.SetActive(true);
    }
}
