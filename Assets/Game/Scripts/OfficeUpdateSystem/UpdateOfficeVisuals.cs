using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///Script has information about which objects get revealed per level
///Display is called at levelup, the regarding objects are then set to visible through that function
///The objects are linked through the Inspector
/// </summary>
/// 

//Data container in which the objectgroups are listed/linked with: groupname (string),
//requiredLevel (int), targetGroup (GameObject), activeWhenUnlocked (bool)
//(decideds if the object should be disabled or enabled once the level is reached).
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
    public PointSliderVisualUpdater ProgressBar;

    [Header("LevelText")]
    public TextMeshProUGUI LevelText;


    public void Display(int currentPoints, int currentLevel, int maxpoints)
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
        {
           ProgressBar.AdjustPoints(currentPoints, maxpoints);
        }
    }


}


