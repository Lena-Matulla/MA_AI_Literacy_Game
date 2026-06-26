using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    [SerializeField]
    public GameObject CorrectPopUp;


    public void AnimationCorrectAnswer()
    {
        CorrectPopUp.SetActive(true);
    }
}
