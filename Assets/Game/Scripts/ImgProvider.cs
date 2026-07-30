using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Video;
using Object = UnityEngine.Object;


/*
public enum ImgCategory
{
    Human,
    Animals,
    Architecture,
    Text
}
*/

public enum MediaType
{
    Image,
    Video
}
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
    //the fallback videos (have to load differently, because WebGL would not work otherwise
    [Header("Fallback Videos in StreamingAssets/Videos")]

    [SerializeField]
    private List<string> fallbackRealVideos = new List<string>
{
    "D_VR1.mp4",
    "D_VR2.mp4"
};

    [SerializeField]
    private List<string> fallbackFakeVideos = new List<string>
{
    "D_VF1.mp4",
    "D_VF2.mp4"
};


    public void Initialize()
    {
        if (isInitialized)
            return;

        allImages = new List<ImgEntry>();

//chosen before the build
#if UNITY_WEBGL && !UNITY_EDITOR
    //Browser version always uses fallback media
    LoadFallbackFolders();
#else
    //exe first checks for external ImageData folder
        basePath = Path.Combine(
            Directory.GetParent(Application.dataPath).FullName,
            "ImageData"
        );

        

        LoadFolders();

#endif

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


    private void LoadFallbackFolders()
    {
        Debug.Log("Loading fallback images from Resources " +
        "and fallback videos from StreamingAssets.");

        //Load fallback images
        foreach (string categoryName in fallbackCategories)
        {
            LoadFallbackResources(
                categoryName + "/Real",
                false,
                categoryName
            );

            LoadFallbackResources(
                categoryName + "/Fake",
                true,
                categoryName
            );
        }
        //Load the videos
        LoadFallbackVideos();
    }
    //Fallback videos for the webgl solution have to be loaded differently
    private void LoadFallbackVideos()
    {
        foreach (string fileName in fallbackRealVideos)
        {
            AddFallbackVideo(fileName, false);
        }

        foreach (string fileName in fallbackFakeVideos)
        {
            AddFallbackVideo(fileName, true);
        }
    }

    private void AddFallbackVideo(string fileName, bool isFake)
    {
        string videoUrl;

#if UNITY_WEBGL && !UNITY_EDITOR

    // WebGL: StreamingAssets is an HTTP URL
    videoUrl =
        Application.streamingAssetsPath.TrimEnd('/') +
        "/Video/" +
        fileName;

#else

        // Editor and EXE: StreamingAssets is a local file path
        string videoPath = Path.Combine(
            Application.streamingAssetsPath,
            "Video",
            fileName
        );

        if (!File.Exists(videoPath))
        {
            Debug.LogError("Fallback video not found: " + videoPath);
            return;
        }

        FileInfo fileInfo = new FileInfo(videoPath);

        if (fileInfo.Length == 0)
        {
            Debug.LogError("Fallback video is empty: " + videoPath);
            return;
        }

        videoUrl = new Uri(videoPath).AbsoluteUri;

#endif
        ImgEntry entry = new ImgEntry
        {
            texture = null,
            videoUrl = videoUrl,
            filePath = null,
            fileName = fileName,
            isFake = isFake,
            category = "Video",
            mediaType = MediaType.Video,
            id = CreateImageId("Video", isFake, fileName)
        };

        allImages.Add(entry);

        Debug.Log("Loaded fallback video: " + videoUrl);
    }


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
                "Loading fallback media."
            );

            LoadFallbackFolders();
        }
    }

    //Loads the images of correct category and isFake state into allImages
    //checks if there is sth in the folders or not. Otherwise it loads the FallbackImages
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

    /*
    private void MakeDictionary()
    {
        categorizedImages = allImages.GroupBy(i => i.category).ToDictionary(g => g.Key, g => g.ToList());
    }
    */

    // builds a list with the order of the images chosen in the study
    // this makes sure that it is random but still balanced
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
        

        /*
        targetImage.texture = videoRenderTexture;

        videoPlayer.Stop();

        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.clip = video;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = videoRenderTexture;
        videoPlayer.isLooping = true;

        videoPlayer.Play();*/
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


    //creates ID for images
    private string CreateImageId(string category, bool isFake, string fileName)
    {
        string realFakeName = isFake ? "Fake" : "Real";

        return category + "/" + realFakeName + "/" + fileName;
    }

   
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

    private void FinishStudy()
    {
        Debug.Log("Study finished. No unused images left. -> reshuffle");

        playthroughs++;

        /*
        if (targetImage != null)
            targetImage.texture = null;

        CurrentImgEntry = null;
        currentIsFake = false;
        currentImgName = "";
        currentCategory = "";

        SaveStudyProgress();

        onStudyFinished?.Invoke();
        */
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



    //old version of LoadRandomImage()
    /*
    
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
    */
}

