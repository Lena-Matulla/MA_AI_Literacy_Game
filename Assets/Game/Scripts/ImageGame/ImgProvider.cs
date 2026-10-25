using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Video;
using Object = UnityEngine.Object;

/// <summary>
///Provides and updates the RawImage with the pictures or videos which are displayed and used in the ImageGame game.
///Also makes the media accessible through the ImageData folder beside the exe.
///Creates a random but balanced study order out of the available categories and the real and fake media.
/// </summary>


//enum which defines the two types of media
public enum MediaType
{
    Image,
    Video
}

//container class for the media to store all relevant data
public class ImgEntry
{
    //for images, if no image then it is null
    public Texture2D texture;
    //for videos, if no video then it is null
    public string videoUrl;
    public string filePath;
    public string fileName;
    public bool isFake;
    public string category;
    public string id;
    public MediaType mediaType;
}


public class ImgProvider : MonoBehaviour
{
    [SerializeField]
    public VideoPlayer videoPlayer;
    [SerializeField]
    public RenderTexture videoRenderTexture;

    //Reference to the Audio to mute the music while video is playing
    [SerializeField]
    public AudioSource AudioSource;
    [Header("Reference to RawImage for display")]
    public RawImage targetImage;
    private List<ImgEntry> allImages;
    Dictionary<string, List<ImgEntry>> categorizedImages;

    //to make a random but balanced study order of images
    private List<ImgEntry> studyOrder;
    private int studyIndex = 0;

    public ImgEntry CurrentImgEntry {  get; private set; }
    public bool currentIsFake { get; private set; }
    public string currentImgName { get; private set; }
    public string currentCategory {  get; private set; }
    public MediaType currentMediaType { get; private set; }
    private string basePath;
    private bool isInitialized = false;

    [Header("Study Events")]
    public UnityEvent onStudyFinished;

    public bool IsStudyFinished { get; private set; }

    public int playthroughs { get; private set; }

    [Header("Do Videos have sound -> Need overall sound to mute?")]
    public bool muteMusicWhileVideo = false;

    //the fallback images
    [SerializeField]
    private List<string> fallbackCategories = new List<string>
    {
        "Human",
        "Animals",
        "Architecture",
        "Text"
    };


    //initializes the list and tries to load images
    //If no media could be loaded, a Debug warning is called and returned.
    //also tries to load saved study progress to reaccess last point in image list
    //to start from the next image. Otherwise calls function to create new study order
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

        if (!TryLoadStudyProgress())
        {
            BuildBalancedStudyOrder();
            IsStudyFinished = false;
            SaveStudyProgress();
        }

        isInitialized = true;
    }

    private void Start()
    {
        Initialize();
    }

    //creates ImageData folder if none exists
    //two cases: 1 -> images in ImageData:
    //      For every category(Folder) call LoadRealOrFakeExternal() to load fake and real images
    //      Categories are only used if both fake and real images are there
    //           2 -> No external images:
    //      LoadFallbackResources() is called to load the fallback images
    private void LoadFolders()
    {
        Directory.CreateDirectory(basePath);

        string[] categoryFolders = Directory.GetDirectories(basePath);

        bool loadedExternalImages = false;

        //Case 1: if there is an ImageData folder besides the exe AND it contains folders which define the categories
        if (categoryFolders.Length > 0)
        {
            foreach (string categoryFolder in categoryFolders)
            {
                string categoryName = Path.GetFileName(categoryFolder);

                int realCount = LoadRealOrFakeExternal(categoryName, false);
                int fakeCount = LoadRealOrFakeExternal(categoryName, true);


                //check if there are images, if not exclude that category
                if (realCount > 0 && fakeCount > 0)
                {
                    loadedExternalImages = true;
                }
                else
                {
                    Debug.LogWarning(
                        $"Category '{categoryName}' is incomplete. " +
                        $"Real: {realCount}, Fake: {fakeCount}. " +
                        "This category will not be used."
                    );

                    allImages.RemoveAll(i => i.category == categoryName);
                }
            }
        }

        //Case 2: No ImageData folder or does not contain images
        //Use fallback images and categories from resources
        if (!loadedExternalImages)
        {
            Debug.LogWarning(
        "No valid external categories found. " +
        "Loading fallback categories from Resources."
    );

            foreach (string categoryName in fallbackCategories)
            {
                LoadFallbackResources(
                    Path.Combine(categoryName, "Real"),
                    false,
                    categoryName
                );

                LoadFallbackResources(
                    Path.Combine(categoryName, "Fake"),
                    true,
                    categoryName
                );
            }
        }
    }

    //Loads the images of given categoryname (foldername) and isFake state (subfoldername) into allImages
    //checks returns number of loaded images
    private int LoadRealOrFakeExternal(string categoryName, bool isFake)
    {
        string realFakeName = isFake ? "Fake" : "Real";
        string folderPath = Path.Combine(basePath, categoryName, realFakeName);

        if (!Directory.Exists(folderPath))
        {
            Debug.LogWarning($"Missing folder: {folderPath}");
            return 0;
        }

        int beforeCount = allImages.Count;

        LoadFolderIntoList(folderPath, isFake, categoryName);

        return allImages.Count - beforeCount;
    }

    //loads media from the folder and stored in the ImgEntry
    private void LoadFolderIntoList(string folderPath, bool isFake, string cat)
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

                    string fileName = Path.GetFileName(file);

                    ImgEntry entry = new ImgEntry
                    {
                        texture = tex,
                        filePath = file,
                        fileName = Path.GetFileName(file),
                        isFake = isFake,
                        category = cat,
                        id = CreateImageId(cat, isFake, fileName)
                    };

                    allImages.Add(entry);
                }
                else
                {
                    Debug.LogWarning("Could not load image: " + file);
                    Destroy(tex);
                }
            }else if(extension == ".mp4")
            {
                string fileName = Path.GetFileName(file);

                ImgEntry entry = new ImgEntry
                {
                    texture = null,
                    //Converts normal file path to something VideoPlayer can use
                    videoUrl = new Uri(file).AbsoluteUri,
                    filePath = file,
                    fileName = fileName,
                    isFake = isFake,
                    category = cat,
                    mediaType = MediaType.Video,
                    id = CreateImageId(cat, isFake, fileName)
                };

                allImages.Add(entry);

            }
        }
    }

    //loads fallback images from resources folder and stores them in ImgEntry
    private void LoadFallbackResources(string resourcesFolder, bool isFake, string cat)
    {
        Texture2D[] fallbackTextures = Resources.LoadAll<Texture2D>(resourcesFolder);

        foreach (Texture2D fallbackTexture in fallbackTextures)
        {

            string fName = fallbackTexture.name + ".jpg";
            ImgEntry entry = new ImgEntry
            {
                texture = fallbackTexture,
                filePath = null,
                fileName = fName,
                isFake = isFake,
                category = cat,
                mediaType = MediaType.Image,
                id = CreateImageId(cat, isFake, fName)
            };
            allImages.Add(entry);
        }
    }


    //builds a list with the order of the images chosen in the study
    //this makes sure that it is random but still balanced
    //stores the ImgEntrys in the list
    private void BuildBalancedStudyOrder()
    {
        studyOrder = new List<ImgEntry>();
        studyIndex = 0;

        List<List<ImgEntry>> groups = new List<List<ImgEntry>>();

        //for each category get a list of the real imgEntry and fake imgEntry
        foreach (string category in allImages.Select(i => i.category).Distinct())
        {
            List<ImgEntry> realGroup = allImages
            .Where(i => i.category == category && i.isFake == false)
            .OrderBy(i => UnityEngine.Random.value)
            .ToList();

            List<ImgEntry> fakeGroup = allImages
                .Where(i => i.category == category && i.isFake == true)
                .OrderBy(i => UnityEngine.Random.value)
                .ToList();

            if (realGroup.Count > 0)
                groups.Add(realGroup);

            if (fakeGroup.Count > 0)
                groups.Add(fakeGroup);
        }

        bool stillHasImages = true;

        // creates a randomized but balanced order, by repeatedly taking one image
        // from each category/fake-real group until all images are used.
        while (stillHasImages)
        {
            stillHasImages = false;

            List<List<ImgEntry>> shuffledGroups = groups
                .Where(g => g.Count > 0)
                .OrderBy(g => UnityEngine.Random.value)
                .ToList();

            foreach (List<ImgEntry> group in shuffledGroups)
            {
                ImgEntry entry = group[0];
                group.RemoveAt(0);

                studyOrder.Add(entry);
                stillHasImages = true;
            }
        }

    }

    //next image of the balanced study order list
    //handles the different media types
    //stores the current media informations under chosenEntry
    public bool LoadNextStudyImage()
    {
        Initialize();

        if (studyOrder == null || studyOrder.Count == 0)
        {
            Debug.LogWarning("Study order is empty.");
            FinishStudy();
            return false;
        }

        if (studyIndex >= studyOrder.Count)
        {
            FinishStudy();
            return false;
        }

        ImgEntry chosenEntry = studyOrder[studyIndex];

        CurrentImgEntry = chosenEntry;
        currentIsFake = chosenEntry.isFake;
        currentImgName = chosenEntry.fileName;
        currentCategory = chosenEntry.category;

        if (chosenEntry.mediaType == MediaType.Image)
        {

            StopVideo();
            targetImage.texture = chosenEntry.texture;
        }else if(chosenEntry.mediaType == MediaType.Video)
        {

            PlayVideo(chosenEntry.videoUrl);
        }

        currentMediaType = chosenEntry.mediaType;


            Debug.Log("Loaded media: " + chosenEntry.fileName);

        return true;
    }

    //handles the videos
    private void PlayVideo(string videoURL)
    {
        if (!isActiveAndEnabled)
        {
            StopVideo();
            return;
        }

        videoPlayer.Stop();

        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoURL;

        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = videoRenderTexture;

        targetImage.texture = videoRenderTexture;

        //makes background sound to mute for the video
        
        if (AudioSource != null && muteMusicWhileVideo)
        {
            AudioSource.mute = true;
        }

        videoPlayer.Play();
        
    }

    private void StopVideo()
    {
        
        if (AudioSource != null && muteMusicWhileVideo)
        {
            // makes background sound go unmute again
            AudioSource.mute = false;
        }

        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }
    }


    //creates unique ID for each media
    private string CreateImageId(string category, bool isFake, string fileName)
    {
        string realFakeName = isFake ? "Fake" : "Real";

        return category + "/" + realFakeName + "/" + fileName;
    }

   
    //when called -> increases study index and calls progress update
    public void ConfirmCurrentImageCompleted()
    {
        if (IsStudyFinished)
            return;

        studyIndex++;
        SaveStudyProgress();

        if (studyIndex >= studyOrder.Count)
        {
            FinishStudy();
        }
    }

    //if balanced list is through -> update playthroughs and reshuffle
    private void FinishStudy()
    {
        Debug.Log("Study finished. No unused images left. -> reshuffle");

        playthroughs++;

        BuildBalancedStudyOrder();
        IsStudyFinished = false;
        SaveStudyProgress();


    }

    //save current progress of images
    private void SaveStudyProgress()
    {
        GameProgressData data = SaveManager.Load();

        data.studyOrderImageIds = studyOrder.Select(i => i.id).ToList();
        data.studyIndex = studyIndex;
        data.studyFinished = IsStudyFinished;
        data.playthroughs = playthroughs;

        SaveManager.Save(data);
    }

    //Load current progress of study images
    private bool TryLoadStudyProgress()
    {
        GameProgressData data = SaveManager.Load();

        playthroughs = data.playthroughs;

        if (data.studyOrderImageIds == null || data.studyOrderImageIds.Count == 0)
            return false;

        Dictionary<string, ImgEntry> imageLookup = allImages.ToDictionary(i => i.id, i => i);

        List<ImgEntry> loadedOrder = new List<ImgEntry>();

        bool missingSavedImage = false;

        foreach (string id in data.studyOrderImageIds)
        {
            if (imageLookup.TryGetValue(id, out ImgEntry entry))
            {
                loadedOrder.Add(entry);
            }
            else
            {
                Debug.LogWarning("Saved image was not found anymore: " + id);
                missingSavedImage = true;
            }
        }

        
        if (missingSavedImage)
        {
            Debug.LogWarning("Image dataset changed. Rebuilding study order.");
            return false;
        }

        if (loadedOrder.Count == 0)
            return false;

        studyOrder = loadedOrder;
        studyIndex = Mathf.Clamp(data.studyIndex, 0, studyOrder.Count);
        IsStudyFinished = data.studyFinished || studyIndex >= studyOrder.Count;

        Debug.Log("Loaded study progress: " + studyIndex + " / " + studyOrder.Count);

        return true;
    }


    //To handle the case that the ImgGame gets closed and reopend on an video
    private void OnDisable()
    {
        if (AudioSource != null && muteMusicWhileVideo)
        {
            // makes background sound go unmute again
            AudioSource.mute = false;
        }


        if (videoPlayer != null)
        {

            
            videoPlayer.Stop();
        }
    }

    private void OnEnable()
    {
        if (CurrentImgEntry != null && CurrentImgEntry.mediaType == MediaType.Video)
        {
            PlayVideo(CurrentImgEntry.videoUrl);
        } else
        {
            StopVideo();
        }
    }

}

