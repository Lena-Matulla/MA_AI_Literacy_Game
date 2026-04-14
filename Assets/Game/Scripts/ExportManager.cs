using UnityEngine;
using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;


public class ExportManager : MonoBehaviour
{

    public DataLogger dataLogger;

    private List<ImageEntry> usedImages = new List<ImageEntry>();

    //register new imageentry, but check if it is already in the list. every image is only stored once
    //regardless of how often it was used
    public void RegisterUsedImage(ImageEntry imageEntry)
    {
        if(imageEntry == null)
        {
            return;
        }

        bool alreadyAdded = usedImages.Exists(x =>
            x.fileName == imageEntry.fileName &&
            x.isFake == imageEntry.isFake &&
            x.filePath == imageEntry.filePath
            );

        if( !alreadyAdded )
        {
            usedImages.Add(imageEntry);
        }
    }

    //handles for example duplicate filenames
    //makes sure to not overwrite existing file and searches for a new name
    //if images/real/cat_1.png, images/real/cat_2.png exist, it makes images/real/cat_3.png as new filepath
    private string EnsureUniquePath(string path)
    {
        //if file does not exist, just use the new one
        if(!File.Exists(path))
        {
            return path;
        }

        //split path so that a new name can be generated
        string directory = Path.GetDirectoryName(path);
        string name = Path.GetFileNameWithoutExtension(path);
        string extension = Path.GetExtension(path);

        //try new filenames by adding numbers till a free name is found 
        int counter = 1;
        string newPath;

        do
        {
            newPath = Path.Combine(directory, $"{name}_{counter}{extension}");
            counter++;
        }
        while (File.Exists(newPath));

        //return safe filename
        return newPath;

    }

    //helper function which helps to make the fallback images readable
    private Texture2D MakeTextureReadable(Texture2D source)
    {
        RenderTexture rt = RenderTexture.GetTemporary(
            source.width,
            source.height,
            0,
            RenderTextureFormat.ARGB32,
            RenderTextureReadWrite.Default
        );

        Graphics.Blit(source, rt);

        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D readableTexture = new Texture2D(
            source.width,
            source.height,
            TextureFormat.RGBA32,
            false
        );

        readableTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        readableTexture.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);

        return readableTexture;
    }

    public void ExportSessionZip()
    {
        try
        {
            //No datalogger found
            if(dataLogger == null)
            {
                Debug.LogError("Export failed: Datalogger missing.");
                return;
            }

            //Log file not found
            if(string.IsNullOrWhiteSpace(dataLogger.FilePath) || !File.Exists(dataLogger.FilePath))
            {
                Debug.LogError("Export failed: Log file not found.");
                return;
            }

            string sessionId = dataLogger.SessionId;
            string tempRoot = Path.Combine(Path.GetTempPath(), "DeepfakeStudyExports");
            string exportFolder = Path.Combine(tempRoot, $"export_{sessionId}");
            string imagesFolder = Path.Combine(exportFolder, "images");
            string realFolder = Path.Combine(imagesFolder, "real");
            string fakeFolder = Path.Combine(imagesFolder, "fake");

            if (Directory.Exists(exportFolder)) 
            {
                Directory.Delete(exportFolder, true);
            }

            Directory.CreateDirectory(exportFolder);
            Directory.CreateDirectory(imagesFolder);
            Directory.CreateDirectory(realFolder);
            Directory.CreateDirectory(fakeFolder);

            //Here the CSV is copied:
            string csvTargetPath = Path.Combine(exportFolder, Path.GetFileName(dataLogger.FilePath));
            File.Copy(dataLogger.FilePath, csvTargetPath, true);

            Debug.Log("used images count: " + usedImages.Count);

            //export used images
            foreach(var img in usedImages)
            {
                if(img == null || img.texture == null) 
                {
                    continue;
                }
                string targetDir = img.isFake ? fakeFolder : realFolder;
                string targetPath = Path.Combine(targetDir, img.fileName);

                //image from disk ( not fallback images), here the original file is copied
                if(!string.IsNullOrEmpty(img.filePath) && File.Exists(img.filePath))
                {
                    if (!File.Exists(targetPath))
                    {
                        File.Copy(img.filePath, targetPath, false);
                    }
                }
                else
                {
                    //fallback images, here it is saved as png (must be done because it lies in the resources folder)
                    targetPath = Path.ChangeExtension(targetPath, ".png");

                    if (!File.Exists(targetPath))
                    {
                        //byte[] pngBytes = img.texture.EncodeToPNG();
                        //File.WriteAllBytes(targetPath, pngBytes);
                        Texture2D readableCopy = MakeTextureReadable(img.texture);
                        byte[] pngBytes = readableCopy.EncodeToPNG();
                        File.WriteAllBytes(targetPath, pngBytes);
                        Destroy(readableCopy);
                    }
                }
            }

            //info file
            string infoPath = Path.Combine(exportFolder, "export_info.txt");
            File.WriteAllText(infoPath,
                $"SessionId: {sessionId}\n" +
                $"ExportedAt: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n" +
                $"UsedImageCount: {usedImages.Count}\n");

            //Downlaods folder
            string downloadsFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            Directory.CreateDirectory(downloadsFolder);
            string zipPath = Path.Combine(downloadsFolder, $"DeepfakeStudy_Export_{sessionId}.zip");

            if (File.Exists(zipPath))
                File.Delete(zipPath);

            ZipFile.CreateFromDirectory(exportFolder, zipPath);
            Debug.Log("Export successful: " + zipPath);

        }
        catch (Exception ex)
        {
            Debug.LogError("Export failed: " + ex.Message);
        }
    }
}
