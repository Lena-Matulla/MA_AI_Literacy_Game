using System.Collections;
using TMPro;
using UnityEngine;

//Called from TrialManager when the correct answer was given
//Starts the animation of the Thumbs Up and updates the score on the computer screen
//score is updated after a delay to fit the animation time :D
public class OnCorrectAnswer : MonoBehaviour
{
    [SerializeField]
    public PopUpManager popUpManager;
    [SerializeField]
    private float scoreDelay = 0.7f;
    [SerializeField] private TextMeshProUGUI ScoreThisRoundTextField;

    public void HandleCorrectAnswer(int ScoreThisRound)
    {
        StartCoroutine(CorrectAnswerSequence(ScoreThisRound));
    }

    private IEnumerator CorrectAnswerSequence(int ScoreThisRound)
    {
        popUpManager.AnimationCorrectAnswer();
        yield return new WaitForSeconds(scoreDelay);
        ScoreThisRoundTextField.text = ScoreThisRound.ToString();
    }
}
