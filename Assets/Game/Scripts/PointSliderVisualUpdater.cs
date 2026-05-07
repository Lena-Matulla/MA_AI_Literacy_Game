using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class PointSliderVisualUpdater : MonoBehaviour
{
    private Slider slider;
    Coroutine currentAnimation;
    private bool initialized = false;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }


    public void AdjustPoints(int points, int maxpoints)
    {
        //check if just started, otherwise it would already animate a full progress bar load because 
        //the initial maxpoint is set to sth different.
        if (!initialized)
        {
            slider.maxValue = maxpoints;
            slider.value = points;
            initialized = true;
            return;
        }

        bool newLevelreached = slider.maxValue != maxpoints;
        int lastLevelMax = (int)slider.maxValue;

        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }
        currentAnimation = StartCoroutine(AnimateBar(points, maxpoints, lastLevelMax, newLevelreached));

    }

    IEnumerator AnimateBar(int points, int newMaxPoints, int oldMaxPoints, bool newLevelreached)
    {
        float duration = 1.5f;

        if (newLevelreached)
        {
            //Fill old bar 
            yield return AnimateSlider(slider.value, oldMaxPoints, duration);

            //reset for new level
            slider.value = 0;
            slider.maxValue = newMaxPoints;

            //animate leftover points
            yield return AnimateSlider(0, points, duration);
        }
        else
        {
            slider.maxValue = newMaxPoints;
            yield return AnimateSlider(slider.value, points, duration);
        }

        slider.value = points;
    }

    IEnumerator AnimateSlider(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            //interpolate based on current time point based on elapsed / duration
            slider.value = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;

        }

        slider.value = to;

    }
}
