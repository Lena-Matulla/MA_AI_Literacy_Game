using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class ImageEntry
{
    public Texture2D texture;
    public string filePath;
    public string fileName;
    public bool isFake;
}

public class ImageProvider : MonoBehaviour
{
    [Header("Reference to UI")]
    public RawImage targetImage;

    private List<ImageEntry> realImages = new List<ImageEntry>();
    private List<ImageEntry> fakeImages = new List<ImageEntry>();

    public ImageEntry CurrentImageEntry { get; private set; }
    public bool currentIsFake { get; private set; }
    public string currentImageName { get; private set; }

    private string realFolderPath;
    private string fakeFolderPath;


    void Start()
    {
        //Folder sits beside exe (Game.exe ImageData/real ImageData/fake)
        string basePath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "ImageData");
        realFolderPath = Path.Combine(basePath, "real");
        fakeFolderPath = Path.Combine(basePath, "fake");

        EnsureFoldersExist();
        LoadImagesFromDisk();

        //fallback if user folders are empty -> fallback images 
        if (realImages.Count == 0)
        {
            Debug.Log("No real images found. Using fallback images.");
            LoadFallbackResources("Real", realImages, false);
        }

        if (fakeImages.Count == 0)
        {
            Debug.Log("No fake images found. Using fallback images.");
            LoadFallbackResources("Fake", fakeImages, true);
        }

        LoadRandomImage();
    }

    void EnsureFoldersExist()
    {
        if (!Directory.Exists(realFolderPath))
            Directory.CreateDirectory(realFolderPath);

        if (!Directory.Exists(fakeFolderPath))
            Directory.CreateDirectory(fakeFolderPath);

        Debug.Log("Real folder: " + realFolderPath);
        Debug.Log("Fake folder: " + fakeFolderPath);
    }

    void LoadImagesFromDisk()
    {
        realImages.Clear();
        fakeImages.Clear();

        LoadFolderIntoList(realFolderPath, realImages, false);
        LoadFolderIntoList(fakeFolderPath, fakeImages, true);

        Debug.Log($"Loaded {realImages.Count} real images and {fakeImages.Count} fake images.");
    }

    void LoadFolderIntoList(string folderPath, List<ImageEntry> targetList, bool isFake)
    {
        if (!Directory.Exists(folderPath))
            return;

        string[] files = Directory.GetFiles(folderPath);
        foreach (string file in files)
        {
            string extension = Path.GetExtension(file).ToLower();

            if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
            {
                byte[] fileData = File.ReadAllBytes(file);
                Texture2D tex = new Texture2D(2,2);

                if (tex.LoadImage(fileData))
                {
                    tex.name = Path.GetFileNameWithoutExtension(file);

                    ImageEntry entry = new ImageEntry
                    {
                        texture = tex,
                        filePath = file,
                        fileName = Path.GetFileName(file),
                        isFake = isFake
                    };

                    targetList.Add(entry);
                }
                else
                {
                    Debug.LogWarning("Could not load image: " + file);
                    Destroy(tex);
                }
            }
        }
    }

    void LoadFallbackResources(string resourcesFolder, List<ImageEntry> targetList, bool isFake)
    {
        Texture2D[] fallbackTextures = Resources.LoadAll<Texture2D>(resourcesFolder);

        foreach (Texture2D fallbackTexture in fallbackTextures)
        {
            //make them into the correct structure
            ImageEntry entry = new ImageEntry
            {
                texture = fallbackTexture,
                filePath = null,
                fileName = fallbackTexture.name + ".png",
                isFake = isFake
            };
            targetList.Add(entry);
        }
    }

    public void LoadRandomImage()
    {
        if (realImages.Count == 0 && fakeImages.Count == 0)
        {
            Debug.LogWarning("No images found in either folder.");
            targetImage.texture = null;
            currentImageName = "";
            CurrentImageEntry = null;
            return;
        }

        //decide random if fake or not fake 
        //LATER CHANGE 
        currentIsFake = Random.value > 0.5f;
        ImageEntry chosenEntry = null;

        if(currentIsFake && fakeImages.Count > 0)
        {
            int index = Random.Range(0, fakeImages.Count);
            chosenEntry = fakeImages[index];
        }
        else if(!currentIsFake && realImages.Count > 0)
        {
            int index = Random.Range(0, realImages.Count);
            chosenEntry = realImages[index];
        }
        else
        {
            Debug.LogWarning("Fake ore Real Folder is empty");
            // fallback if one category is empty
            if (fakeImages.Count > 0)
            {
                currentIsFake = true;
                int index = Random.Range(0, fakeImages.Count);
                chosenEntry = fakeImages[index];
            }
            else if (realImages.Count > 0)
            {
                currentIsFake = false;
                int index = Random.Range(0, realImages.Count);
                chosenEntry = realImages[index];
            }
        }
        CurrentImageEntry = chosenEntry;
     
        //showcase image
        targetImage.texture = chosenEntry != null ? chosenEntry.texture : null;

        currentImageName = chosenEntry != null ? chosenEntry.fileName : "";
    }

    public void ReloadImages()
    {
        LoadImagesFromDisk();

        if (realImages.Count == 0)
            LoadFallbackResources("Real", realImages, false);

        if (fakeImages.Count == 0)
            LoadFallbackResources("Fake", fakeImages, true);
    }
}
