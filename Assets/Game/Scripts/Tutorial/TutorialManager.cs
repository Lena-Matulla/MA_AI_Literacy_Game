using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] gameObjects;
    public int currentPos = 0;
    public Button next;
    public Button back;
    public GameObject click;

    private void OnEnable()
    {
        click.SetActive(false);
    }

    private void OnDisable()
    {
        if (click != null)
        {
            click.SetActive(true);
        }
    }

    public void NextButton()
    {
        if (currentPos < gameObjects.Length - 1)
        {
            currentPos++;
            OpenPage();
        }
        
    }

    public void BackButton()
    {
        if (currentPos > 0)
        {
            currentPos--;
            OpenPage();
        }
    }

    private void OpenPage()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if(i != currentPos)
            {
                gameObjects[i].SetActive(false);
            }
            else
            {
                gameObjects[i].SetActive(true);
            }


            
        }

        if (currentPos == 0)
        {
            back.interactable = false;
        }
        else if (currentPos == gameObjects.Length - 1)
        {
            next.interactable = false;
        }
        else
        {
            next.interactable = true;
            back.interactable = true;
        }
    }

}
