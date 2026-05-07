using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class PointSliderVisualUpdater : MonoBehaviour
{
    private Slider slider;
    Coroutine currentAnimation;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }


    public void AdjustPoints(int points, int maxpoints)
    {
        slider.maxValue = maxpoints;
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }
        currentAnimation = StartCoroutine(AnimateBar(points));
    }

    IEnumerator AnimateBar(int points)
    {
        float elapsed = 0f;
        float duration = 1.5f;
        float pointstart = slider.value;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            //interpolate based on current time point based on elapsed / duration
            slider.value = Mathf.Lerp(pointstart, points, elapsed / duration);

            yield return null;

        }

        slider.value = points;
    }
}
