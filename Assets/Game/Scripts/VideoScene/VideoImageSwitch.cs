using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Object = UnityEngine.Object;

public class VideoImageSwitch : MonoBehaviour
{
    private List<ImgEntry> allImages;
    private string basePath;

    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    public RenderTexture videoRenderTexture;
    private Object[] mediaFiles;

    private void Awake()
    {
        Object[] allFiles = Resources.LoadAll<Object>("VideoData");

        mediaFiles = System.Array.FindAll(allFiles, file =>
            file is Texture2D ||
            file is Sprite ||
            file is VideoClip
        );

        if (mediaFiles.Length == 0)
        {
            Debug.LogWarning("No images or videos found in Resources/" + "VideoData");
        }
    }
    public void nextRandomMedia()
    {
        if (mediaFiles == null || mediaFiles.Length == 0)
            return;

        Object chosenMedia = mediaFiles[Random.Range(0, mediaFiles.Length)];
        DisplayMedia(chosenMedia);
    }

    private void DisplayMedia(Object chosenMedia)
    {
        if (chosenMedia is Texture2D image)
        {
            StopVideo();
            rawImage.texture = image;
        }
        else if (chosenMedia is Sprite sprite)
        {
            StopVideo();
            rawImage.texture = sprite.texture;
        }
        else if (chosenMedia is VideoClip video)
        {
            PlayVideo(video);
        }
    }

    private void PlayVideo(VideoClip video)
    {
        rawImage.texture = videoRenderTexture;

        videoPlayer.Stop();

        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.clip = video;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = videoRenderTexture;
        videoPlayer.isLooping = true;

        videoPlayer.Play();
    }

    private void StopVideo()
    {
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }
    }
}
