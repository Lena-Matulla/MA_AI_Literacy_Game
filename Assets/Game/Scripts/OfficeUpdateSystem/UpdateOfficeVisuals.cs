using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class OfficeUnlockGroup
{
    public string groupName;
    public int requiredLevel;
    public GameObject targetGroup;
    public bool activeWhenUnlocked = true;
}

public class UpdateOfficeVisuals : MonoBehaviour
{

    [Header("Unlock groups")]
    public OfficeUnlockGroup[] unlockGroups;

    [Header("ProgressBar")]
    public UnityEngine.UI.Slider ProgressBar;

    [Header("LevelText")]
    public TextMeshProUGUI LevelText;

    public void Display(int currentPoints, int currentLevel)
    {
        foreach (OfficeUnlockGroup group in unlockGroups)
        {
            if (group.targetGroup == null)
                continue;

            bool unlocked = currentLevel >= group.requiredLevel;
            bool finalState = group.activeWhenUnlocked ? unlocked : !unlocked;

            group.targetGroup.SetActive(finalState);
        }

        if (LevelText != null)
            LevelText.text = currentLevel.ToString();

        if (ProgressBar != null)
            ProgressBar.value = currentPoints;
    }
}
