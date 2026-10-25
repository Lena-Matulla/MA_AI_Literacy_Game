using System.Drawing;
using UnityEngine;

/// <summary>
/// Processes the progress of Points and Levels.
/// Also handles the effects when a new level is reached, saves and loads the progress and updates the office progress visuals.
///aka:Point and Level System
/// </summary>

public class ProgressManager : MonoBehaviour
{

    public static ProgressManager Instance;

    [Header("Progress")]
    public int currentPoints = 0;
    public int currentLevel = 0;

    [Header("Level thresholds (and amount of levels based on length)")]
    public int[] levelThresholds = { 0, 10, 25, 45, 70, 100 };


    [Header("Reference")]
    [SerializeField] private UpdateOfficeVisuals officeVisuals;

    [Header("Reference to the Notebook")]
    public GameObject notebook;


    [Header("Reference to the BookObject for the text")]
    public Book book;

    [Header("Reference to the LevelUp Popup")]
    public PopUpManager LevelUpPopUpManager;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadProgress();
        
    }
    void Start()
    {
        RefreshVisuals();

    }

    //add points, update level, save points and level and refresh Office
    public void AddPoints(int amount)
    {
        currentPoints += amount;
        UpdateLevel();
        SaveProgress();
        RefreshVisuals();
    }

    public void RemovePoints(int amount)
    {
        currentPoints -= amount;

        if (currentPoints < 0)
            currentPoints = 0;

        UpdateLevel();
        SaveProgress();
        RefreshVisuals();
    }

        
    private void UpdateLevel()
    {
        GetLevelFromPoints(currentPoints);
    }

    //checks if enough points for new level are reached, calculates leftover points as well (if enough
    //new points for two levels for example)
    //if new level:
    //      update current points and current level, open notebook, level up pop up animation
    private void GetLevelFromPoints(int points)
    {

        while (currentLevel <= levelThresholds.Length - 1)
        {
            if(currentLevel == levelThresholds.Length - 1)
            {
                int LastLevelNeededPoints = levelThresholds[currentLevel];
                if (currentPoints >= LastLevelNeededPoints)
                {
                    currentPoints = LastLevelNeededPoints;
                    currentLevel = levelThresholds.Length-1;
                    break;
                }
                else
                {
                    //stop if not enough points anymore
                    break;
                }

            }
            else
            {
                int neededPoints = levelThresholds[currentLevel + 1];

                if (currentPoints >= neededPoints)
                {
                    currentPoints -= neededPoints;
                    currentLevel++;


                    notebook.SetActive(true);

                    //update book
                    // book.UnlockNextTextPage();
                    book.UnlockTextPagesUntil(ProgressManager.Instance.currentLevel);


                    //open book
                    book.OpenAtNewestUnlockedText();
                    

                    //play LevelUp PopUp
                    LevelUpPopUpManager.AnimationCorrectAnswer();

                }
                else
                {
                    //stop if not enough points anymore
                    break;
                }
            }

            
        }
    }


    //save points and level through SaveManager
    public void SaveProgress()
    {
        GameProgressData data = SaveManager.Load();

        data.currentPoints = currentPoints;
        data.currentLevel = currentLevel;

        SaveManager.Save(data);
    }
    
    //Load points and level through SaveManager
    public void LoadProgress()
    {
        GameProgressData data = SaveManager.Load();
        currentPoints = data.currentPoints;
        currentLevel = data.currentLevel;
    }
    
    //Update office visuals
    public void RefreshVisuals()
    {
        if (officeVisuals != null)
        {
            if (currentLevel != levelThresholds.Length - 1)
            {
                //last one is the needed points in the level, so the progressbar max gets updated accordingly
                officeVisuals.Display(currentPoints, currentLevel, levelThresholds[currentLevel + 1]);
            }
            else
            {
                officeVisuals.Display(currentPoints, currentLevel, levelThresholds[currentLevel]);
            }
        }
    }

    [ContextMenu("Add 10 Points")]
    private void DebugAdd10Points()
    {
        AddPoints(10);
    }

    [ContextMenu("Reset Progress")]
    public void ResetProgress()
    {
        currentPoints = 0;
        currentLevel = 0;

        //remove file
        SaveManager.DeleteSave();
        //write freh data
        SaveProgress();
        //update UI
        RefreshVisuals();

        Debug.Log("Progress reset");
    }

}
