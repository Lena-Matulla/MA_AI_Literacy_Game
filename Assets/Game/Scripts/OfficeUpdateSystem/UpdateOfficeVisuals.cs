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
    [Header("Progress")]
    public int currentPoints = 0;
    public int currentLevel = 0;

    //TODO: MAYBE CHANGE LATER BASED ON HOW MANY LEVELS
    [Header("Level thresholds")]
    public int[] levelThresholds = { 0, 10, 25, 45, 70, 100 };

    [Header("Unlock groups")]
    public OfficeUnlockGroup[] unlockGroups;

    [Header("ProgressBar")]
    public UnityEngine.UI.Slider ProgressBar;

    [Header("LevelText")]
    public TextMeshProUGUI LevelText;


    private void Start()
    {
        UpdateOffice();
    }

    //this is called later on to change the current points based on answers
    public void AddPoints(int amount)
    {
        currentPoints += amount;
        UpdateLevel();
    }

    //do i remove points? maybe delete later on
    public void RemovePoints(int amount)
    {
        currentPoints -= amount;

        if (currentPoints < 0)
            currentPoints = 0;

        UpdateLevel();
    }

    //check if a new level is archieved and update if necessary
    private void UpdateLevel()
    {
        int newLevel = GetLevelFromPoints(currentPoints);

        if (newLevel != currentLevel)
        {
            currentLevel = newLevel;
            UpdateOffice();
            Debug.Log("Office level changed to: " + currentLevel);
        }
    }

    private int GetLevelFromPoints(int points)
    {
        int level = 0;

        for (int i = 0; i < levelThresholds.Length; i++)
        {
            if (points >= levelThresholds[i])
                level = i;
        }

        return level;
    }

    //activate objects based on level
    private void UpdateOffice()
    {
        foreach (OfficeUnlockGroup group in unlockGroups)
        {
            if (group.targetGroup == null)
                continue;

            bool unlocked = currentLevel >= group.requiredLevel;
            bool finalState = group.activeWhenUnlocked ? unlocked : !unlocked;

            group.targetGroup.SetActive(finalState);
        }
        LevelText.text = currentLevel.ToString();
        ProgressBar.value = currentPoints;
    }

    private void SetGroupActive(GameObject group, bool activeState)
    {
        if (group != null)
            group.SetActive(activeState);
    }

    [ContextMenu("Refresh Office")]
    private void RefreshOffice()
    {
        currentLevel = GetLevelFromPoints(currentPoints);
        UpdateOffice();
    }

    [ContextMenu("Test Add Points")]
    private void TestAddPoints()
    {
        AddPoints(10);
    }
}
