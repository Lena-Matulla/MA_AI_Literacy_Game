# ImgProvider.cs

## Purpose
provides and updates the RawImage with the pictures which are displayed and used in the ImageGame game. 
Also makes the images accessable throught the folders beside the exe. Choses random out of the four categories.

## Used By
ImageProvider (updates Raw Image of the UI ImagePanel)

## Inspector References
| Field | Type | Purpose |
|---|---|---|
| Target Image | Raw Image | The Raw Image of the UI where the image is displayed |

## Classes
- ImgCategory -> ENUM
  - defines the four categories of the images
    - Human
    - Animals
    - Architecture
    - Text
- ImgEntry
  - container class for the images to store all relevant data
  - contains: texture, filePath, fileName,isFake (bool), category (ImgCategory)
- ImgProvider

## Important Methods

### Initialize()
Checks if it was already initialized, if yes then return.

stores the basepath to the parent folder ImageData where the images lie.

initializes the allImages List.
Calls EnsureFolderExists()
Then checks if images are loaded.
Makes a dictionary out of the list and sets initialized to true


### Start()
Calls initialize(). Only effective if LoadRandomImage() is not called beforehand through the TrialManager. (Just Fallback)

### LoadFolders()
calls for each category LoadRealOrFake once for real and once for fake

### LoadRealOrFake(ImgCategory category, string categoryName, bool isFake)
Stores the needed Path and calls LoadFolderintoList() with the path, isFake and category.
before that it stores the current amount of images in AllImages. After the call it gets the number of added images. If it is zero, that means the folder was empty, then the Fallback folder is used. Thats done by calling LoadFallbackResources()


### LoadFolderIntoList(string folderpath, List<ImageEntry> targetList, bool isFake)

Loads the images from the folders if they are .png, .jpg or .jpeg.

Stores them in a ImageEntry which contains: 
texture, filePath, fileName, isFake (bool)

### LoadFallbackResources (string resourcesFolder, List<ImageEntry> targetList, bool isFake)
In case that the folders are empty, this function gets called. It loads the fallback images from the Resources folder of the category and isFake case (fake or real folder).

### MakeDictionary
makes a dictionary with the categories

### LoadRandomImage()
First calls Initialize()

if images are null or count is null, then A Debug warning is called and returned.

It then first choses on category randomly and also checks if the category is empty.

It safes the images of that category in a List.

Choses randomly if real or fake. 
Then stores all images which have the correct category and isFake into a list.

if there are images, then it choses randomly one image from that list.

Sets CurrentImgEntry, currentIsFake, targetImage.texture, currentImgName and currentCategory to the updated values. Those are all needed in Trialmanager (except for the texture)


## Data
Gets the images and stores them in the container class to be used. Chooses images from those to update the RawImage.

## Notes
