using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class MouseClickPosition : MonoBehaviour, IPointerClickHandler
{
    [Header("Refs")]

    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform imageRect;
    [SerializeField] GameObject marker;


    //Results
    public Vector2 lastLocal; //local inside of rect
    public Vector2 lastNormal; //normalized (0-1)
    public bool interacted = false;

    [SerializeField]
    public UnityEvent<Vector2, Vector2> OnImageClicked;



    private void OnDisable()
    {
        interacted = false;
        marker.SetActive(false);
        lastLocal = new Vector2(-1000, -1000);
        lastNormal = new Vector2(-1, -1);
    }

    void Reset()
    {
        imageRect = GetComponent<RectTransform>();
        canvas = GetComponent<Canvas>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (imageRect == null) return;

        //Screen coordinates to in rect coordinates (local)
        Camera cam = null;
        if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay) 
        {
            cam = canvas.worldCamera;
        }

        if ( RectTransformUtility.ScreenPointToLocalPointInRectangle(imageRect,eventData.position, cam, out Vector2 localPoint))
        {
            //local based on rec coordinates
            lastLocal = localPoint;

            //normalizing
            Rect r = imageRect.rect; // UI-values
            float xNorm = (localPoint.x - r.xMin) / r.width;
            float yNorm = (localPoint.y - r.yMin) / r.height;

            lastNormal = new Vector2(xNorm, yNorm);

            Debug.Log($"Local: {lastLocal} | Normalized: {lastNormal}");
        }
        marker.SetActive(true);
        marker.transform.localPosition = lastLocal;
        interacted = true;
        OnImageClicked?.Invoke(lastLocal, lastNormal);
    }

}
