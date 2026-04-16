using UnityEngine;
using UnityEngine.InputSystem;

public class ClickHandler : MonoBehaviour
{

    public Camera deskCam;
    public Camera officeCam;
    
    [SerializeField]
    public GameObject ImageGamePanel;

    void Update()
    {

        //if mouse clicked
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            //take current cam based on screen position
            Camera activeCam;

            if (Mouse.current.position.ReadValue().y < Screen.height / 2f)
            {
                activeCam = deskCam;
            }
            else
            {
                activeCam = officeCam;
            }


            //Screen to world position
            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector2 mouseWorldPos = activeCam.ScreenToWorldPoint(mouseScreenPos);
            //Shoot the ray
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);


            //if hit do
            if (hit.collider != null)
            {
                Debug.Log("Clicked on: " + hit.collider.gameObject.name);

                //access clicked object
                if (hit.collider.CompareTag("Laptop"))
                {
                    //handle interaction
                    Debug.Log("Laptop item clicked!");
                    ImageGamePanel.SetActive(true);
                }
            }
        }
    }
}
