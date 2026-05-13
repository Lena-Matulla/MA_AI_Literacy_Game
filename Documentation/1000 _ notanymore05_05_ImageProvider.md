# ImageProvider.cs

## Purpose
provides and updates the RawImage with the pictures which are displayed and used in the ImageGame game. 
Also makes the images accessable throught the folders beside the exe.

## Used By
ImageProvider (updates Raw Image of the UI ImagePanel)

## Inspector References
| Field | Type | Purpose |
|---|---|---|
| Target Image | Raw Image | The UI where the image is displayed |

## Classes
- ImageEntry
  - container class for the images to store all relevant data
  - contains: texture, filePath, fileName,isFake (bool)
- ImageProvider

## Important Methods
### Start()
stores the needed paths to the folders where the images lie (Folders are beside the exe)

Calls EnsureFolderExists() and LoadImagesFromDisk()

If folders are empty, take the fallback images. They are loaded separately per category (real / fake). 
The folders are besides the exe and can therefore be accessed.
-->> maybe changes later!!

Calls LoadRandomImage()

### EnsureFolderExists()
checks if the folder exist, if not creates them

### LoadImagesFromDisk()
Loads images out of the Folders into lists with the function:
LoadFolderIntoList()

### LoadFolderIntoList(string folderpath, List<ImageEntry> targetList, bool isFake)

Loads the images from the folders if they are .png, .jpg or .jpeg.

Also stores them in a ImageEntry which contains: 
texture, filePath, fileName, isFake (bool)

### LoadFallbackResources (string resourcesFolder, List<ImageEntry> targetList, bool isFake)
In case that the folders are empty, this function gets called. It loads the fallback images from the Resources folder.

### LoadRandomImage()
chooses random between realImages and fakeImages and then chooses the picture random as well. (also covers fallback options in case that the realImages or fakeImages are empty)

### ReloadImages()
Reload the images from disk and fallback resouce. Does not automatically display new image.

## Data
Gets the images and stores them in the container class to be used. Chooses images from those to update the RawImage

## Notes
