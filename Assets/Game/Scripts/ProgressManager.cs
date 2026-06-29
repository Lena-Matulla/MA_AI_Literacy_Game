using UnityEngine;

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RefreshVisuals();

    }

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
                    //update book
                    book.UnlockNextTextPage();
                    //open book
                    book.OpenAtNewestUnlockedText();
                    notebook.SetActive(true);
                }
                else
                {
                    //stop if not enough points anymore
                    break;
                }
            }

            
        }


        /*

        for (int i = 0; i < levelThresholds.Length; i++)
        {
            if (points >= levelThresholds[i])
                level = i;
        }
        
        return level;
        */
    }


    public void SaveProgress()
    {
        GameProgressData data = SaveManager.Load();

        data.currentPoints = currentPoints;
        data.currentLevel = currentLevel;

        SaveManager.Save(data);
    }
    

    public void LoadProgress()
    {
        GameProgressData data = SaveManager.Load();
        currentPoints = data.currentPoints;
        currentLevel = data.currentLevel;
    }
    

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
