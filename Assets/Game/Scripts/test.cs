using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public RawImage rawImage;
    public AspectRatioFitter fitter;

    public void SetTexture(Texture tex)
    {
        rawImage.texture = tex;

        float ratio = (float)tex.width / tex.height;
        fitter.aspectRatio = ratio;
    }
}
