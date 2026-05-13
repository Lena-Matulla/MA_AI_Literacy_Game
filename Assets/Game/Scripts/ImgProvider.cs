using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public enum ImgCategory
{
    Human,
    Animals,
    Architecture,
    Text
}
public class ImgEntry
{
    public Texture2D texture;
    public string filePath;
    public string fileName;
    public bool isFake;
    public ImgCategory category;
}


public class ImgProvider : MonoBehaviour
{
    [Header("Reference to RawImage for display")]
    public RawImage targetImage;

    private List<ImgEntry> allImages;
    Dictionary<ImgCategory, List<ImgEntry>> categorizedImages;

    public ImgEntry CurrentImgEntry {  get; private set; }
    public bool currentIsFake { get; private set; }
    public string currentImgName { get; private set; }

    public string currentCategory {  get; private set; }

    private string basePath;

    private bool isInitialized = false;

    public void Initialize()
    {
        if (isInitialized)
            return;

        basePath = Path.Combine(
            Directory.GetParent(Application.dataPath).FullName,
            "ImageData"
        );

        allImages = new List<ImgEntry>();

        LoadFolders();

        if (allImages.Count == 0)
        {
            Debug.LogWarning("No images loaded.");
            return;
        }

        MakeDictionary();

        isInitialized = true;
    }

    private void Start()
    {
        Initialize();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /*void Awake()
    {
        //basepath to folder with images
        //Folders sit beside exe (Game.exe ImageData)
        basePath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "ImageData");
        allImages = new List<ImgEntry>();
        //allImages.Clear();
        EnsureFolderExist();
        MakeDictionary();
        LoadRandomImage();

    }*/



    private void LoadFolders()
    {
        foreach (ImgCategory category in Enum.GetValues(typeof(ImgCategory)))
        {
            string name = category.ToString();

            LoadRealOrFake(category, name, false);
            LoadRealOrFake(category, name, true);

        }
    }

    //Loads the images of correct category and isFake state into allImages
    //checks if there is sth in the folders or not. Otherwise it loads the FallbackImages
    private void LoadRealOrFake(ImgCategory category, string categoryName, bool isFake)
    {
        string realFakeName = isFake ? "Fake" : "Real";
        string folderPath = Path.Combine(basePath, categoryName, realFakeName);

        Directory.CreateDirectory(folderPath);

        int beforeCount = allImages.Count;

        LoadFolderIntoList(folderPath,isFake, category);

        int loadedCount = allImages.Count - beforeCount;

        if (loadedCount == 0)
        {
            Debug.LogWarning($"No {realFakeName} images found for {categoryName}. Loading fallback.");
            LoadFallbackResources(Path.Combine(categoryName, realFakeName), isFake, category);
        }
    }

    private void LoadFolderIntoList(string folderPath, bool isFake, ImgCategory cat)
    {
        string[] files = Directory.GetFiles(folderPath);
        foreach (string file in files)
        {
            string extension = Path.GetExtension(file).ToLower();

            if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
            {
                byte[] fileData = File.ReadAllBytes(file);
                Texture2D tex = new Texture2D(2, 2);

                if (tex.LoadImage(fileData))
                {
                    tex.name = Path.GetFileNameWithoutExtension(file);

                    ImgEntry entry = new ImgEntry
                    {
                        texture = tex,
                        filePath = file,
                        fileName = Path.GetFileName(file),
                        isFake = isFake,
                        category = cat
                    };

                    allImages.Add(entry);
                }
                else
                {
                    Debug.LogWarning("Could not load image: " + file);
                    Destroy(tex);
                }
            }
        }
    }

    private void LoadFallbackResources(string resourcesFolder, bool isFake, ImgCategory cat)
    {
        Texture2D[] fallbackTextures = Resources.LoadAll<Texture2D>(resourcesFolder);

        foreach (Texture2D fallbackTexture in fallbackTextures)
        {
            ImgEntry entry = new ImgEntry
            {
                texture = fallbackTexture,
                filePath = null,
                fileName = fallbackTexture.name + ".jpg",
                isFake = isFake,
                category = cat
            };
            allImages.Add(entry);
        }
    }

    private void MakeDictionary()
    {
        categorizedImages = allImages.GroupBy(i => i.category).ToDictionary(g => g.Key, g => g.ToList());
    }

    public void LoadRandomImage()
    {
        Initialize();

        if (categorizedImages == null || categorizedImages.Count == 0)
        {
            Debug.LogWarning("categorizedImages is empty or not initialized.");
            return;
        }

        //chose one category randomly
        ImgCategory[] categories = (ImgCategory[])Enum.GetValues(typeof(ImgCategory));

        ImgCategory randomCategory = categories[UnityEngine.Random.Range(0, categories.Length)];

        //fallback if category has no images
        if (!categorizedImages.ContainsKey(randomCategory))
        {
            Debug.LogWarning("No images for category: " + randomCategory);
            return;
        }

        List<ImgEntry> images = categorizedImages[randomCategory];

        bool chooseFake = UnityEngine.Random.value > 0.5f;

        //get images with correct category and correct bool
        List<ImgEntry> matchingImages = images.Where(i => i.isFake == chooseFake).ToList();
        ImgEntry chosenEntry = null;

        if (matchingImages.Count > 0)
        {
            chosenEntry = matchingImages[UnityEngine.Random.Range(0, matchingImages.Count)];

            Debug.Log(chosenEntry.fileName);
        }
        else
        {
            Debug.LogWarning("No matching images found.");
        }

        CurrentImgEntry = chosenEntry;
        currentIsFake = chooseFake;
        targetImage.texture = chosenEntry != null ? chosenEntry.texture : null;
        currentImgName = chosenEntry != null ? chosenEntry.fileName : "";
        currentCategory = randomCategory.ToString();

    }
}
