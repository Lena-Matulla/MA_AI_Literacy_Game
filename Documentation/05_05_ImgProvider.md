# ImgProvider.cs

## Purpose

Provides and updates the RawImage with the pictures or videos which are displayed and used in the ImageGame game.
Also makes the media accessible through the ImageData folder beside the exe. In the WebGL version the fallback media is used instead.

Creates a random but balanced study order out of the available categories and the real and fake media.

## Used By

ImageProvider (updates the Raw Image of the UI ImagePanel and plays videos)

## Inspector References

| Field                  | Type          | Purpose                                                                                   |
| ---------------------- | ------------- | ----------------------------------------------------------------------------------------- |
| Video Player           | VideoPlayer   | Plays the video files                                                                     |
| Video Render Texture   | RenderTexture | Stores the video output which is displayed on the Raw Image                               |
| Audio Source           | AudioSource   | Reference to the background audio which can be muted while a video is playing             |
| Target Image           | RawImage      | The Raw Image of the UI where the image or video is displayed                             |
| On Study Finished      | UnityEvent    | Event for when the study is finished. Currently not called in the active FinishStudy code |
| Mute Music While Video | bool          | Defines if the Audio Source should be muted while a video is playing                      |
| Fallback Categories    | List<string>  | Defines the categories which are loaded from the Resources fallback folders               |
| Fallback Real Videos   | List<string>  | Contains the filenames of the fallback real videos                                        |
| Fallback Fake Videos   | List<string>  | Contains the filenames of the fallback fake videos                                        |

## Classes

- MediaType -> ENUM

  - defines the two types of media

    - Image
    - Video
- ImgEntry

  - container class for the media to store all relevant data
  - contains: texture, videoUrl, filePath, fileName, isFake (bool), category (string), id and mediaType
  - texture is null when the entry is a video
  - videoUrl is null when the entry is an image
- ImgProvider

## Important Methods

### Initialize()

Checks if it was already initialized, if yes then return.

Initializes the allImages List.

In the WebGL build it directly calls LoadFallbackFolders(), because external folders cannot be used there.

In the editor or exe version it stores the basepath to the parent folder ImageData where the media lies and calls LoadFolders().

If no media could be loaded, a Debug warning is called and returned.

Tries to load the saved study progress by calling TryLoadStudyProgress().

If there is no valid saved progress, it calls BuildBalancedStudyOrder(), sets IsStudyFinished to false and saves the new progress.

Then sets initialized to true.

### Start()

Calls Initialize(). Only effective if LoadNextStudyImage() is not called beforehand through the TrialManager. (Just Fallback)

### LoadFallbackFolders()

Loads the fallback images for every category from the Resources folders.

For every category it loads the Real and Fake folder by calling LoadFallbackResources().

After that it calls LoadFallbackVideos() to add the fallback videos from StreamingAssets.

### LoadFallbackVideos()

Calls AddFallbackVideo() for every filename in the fallbackRealVideos and fallbackFakeVideos Lists.

### AddFallbackVideo(string fileName, bool isFake)

Creates the correct URL for a fallback video.

In WebGL the StreamingAssets HTTP URL is used.

In the editor or exe version it checks if the local file exists and if the file is not empty.

Stores the video in an ImgEntry with the category Video and adds it to allImages.

### LoadFolders()

Creates the ImageData folder if it does not exist.

Gets all folders inside ImageData. Every folder is treated as one category.

For every category it calls LoadRealOrFakeExternal() once for the Real folder and once for the Fake folder.

A category is only used when it contains at least one real and one fake media entry. If one of them is missing, all entries of that category are removed.

If there are no valid external categories, LoadFallbackFolders() is called.

The external folder structure is:

ImageData/CategoryName/Real
ImageData/CategoryName/Fake

### LoadRealOrFakeExternal(string categoryName, bool isFake)

Creates the needed path to the Real or Fake folder of the category.

If the folder does not exist, it calls a Debug warning and returns zero.

Stores the current amount of media in allImages and calls LoadFolderIntoList().

Returns the number of media entries which were added.

### LoadFolderIntoList(string folderPath, bool isFake, string cat)

Loads the media from the folder.

Supported image formats are:

- .png
- .jpg
- .jpeg

The image data is loaded into a Texture2D and stored in an ImgEntry.

Also loads .mp4 video files.

Videos are stored with a videoUrl instead of a texture and their mediaType is set to Video.

Every entry contains its file path, filename, real or fake state, category and a created ID.

### LoadFallbackResources(string resourcesFolder, bool isFake, string cat)

Loads all Texture2D fallback images from the given Resources folder.

Stores every texture in an ImgEntry with its fake state, category, media type and ID.

Adds the entries to allImages.

### BuildBalancedStudyOrder()

Creates a new List which contains the order of the media for the study.

First separates the media into groups for every category and real or fake state.

The entries inside every group are randomized.

Then repeatedly takes one entry from every available group. The order of the groups is also randomized each time.

This makes the study order random but still balanced between the categories and between real and fake media.

### LoadNextStudyImage()

First calls Initialize().

If the study order is null or empty, a Debug warning is called and FinishStudy() is called.

If every entry of the study order was already used, FinishStudy() is called.

Gets the next ImgEntry from the studyOrder List and sets:

- CurrentImgEntry
- currentIsFake
- currentImgName
- currentCategory
- currentMediaType

If the entry is an image, StopVideo() is called and the texture of the Target Image is updated.

If the entry is a video, PlayVideo() is called with the video URL.

Returns true if media was loaded and false if no media could be loaded.

### PlayVideo(string videoURL)

Stops the current video and sets the VideoPlayer source to the given URL.

Sets the VideoPlayer output to the videoRenderTexture and displays it on the Target Image.

If muteMusicWhileVideo is true, the Audio Source is muted.

Then starts playing the video.

### StopVideo()

Unmutes the Audio Source if muteMusicWhileVideo is active.

Stops the VideoPlayer if a video is currently playing.

### CreateImageId(string category, bool isFake, string fileName)

Creates a unique ID for an entry.

The ID consists of:

Category/RealOrFake/FileName

The ID is used to restore the saved study order.

### ConfirmCurrentImageCompleted()

Checks if the study is already finished.

Increases the studyIndex and saves the study progress.

If every entry of the current study order was completed, FinishStudy() is called.

### FinishStudy()

Increases the amount of playthroughs.

Creates a new randomized and balanced study order.

Sets IsStudyFinished to false and saves the new study progress.

This means that after every complete playthrough the media is reshuffled and the study starts again.

### SaveStudyProgress()

Loads the current GameProgressData from the SaveManager.

Stores:

- the IDs of the study order
- the current studyIndex
- the IsStudyFinished state
- the number of playthroughs

Then saves the updated GameProgressData.

### TryLoadStudyProgress()

Loads the saved GameProgressData.

Creates a dictionary which connects every current media ID with its ImgEntry.

Uses the saved IDs to rebuild the previous study order.

If a saved entry cannot be found because the dataset changed, the saved order is not used and a new study order is created.

Restores the studyIndex, IsStudyFinished state and the amount of playthroughs.

Returns true if the saved progress could be loaded and false if a new study order is needed.

### OnDisable()

Makes sure that the Audio Source is unmuted when the ImgProvider is disabled.

Also stops the VideoPlayer.

This handles the case that the ImageGame gets closed while a video is playing.

### OnEnable()

Checks if the current ImgEntry is a video.

If it is a video, it starts the video again.

Otherwise StopVideo() is called.

## Data

Gets the images and videos and stores them in the ImgEntry container class to be used.

External media is loaded from the ImageData folder in the editor or exe version.

Fallback images are loaded from Resources and fallback videos are loaded from StreamingAssets.

Creates and saves a randomized but balanced study order. Uses the entries from this order to update the RawImage or VideoPlayer.

## Notes

- Categories are strings and are defined by the folder names inside ImageData.
- The old ImgCategory enum is commented out.
- The old MakeDictionary() and LoadRandomImage() implementations are commented out.
- An external category is only used if both its Real and Fake folders contain valid media.
- If no valid external category exists, all fallback media is loaded.
- The WebGL version always uses fallback media.
- Supported external formats are .png, .jpg, .jpeg and .mp4.
- After all media was used, a new balanced order is automatically created and the playthrough counter is increased.
- onStudyFinished is currently not invoked because its call is inside commented-out code.
