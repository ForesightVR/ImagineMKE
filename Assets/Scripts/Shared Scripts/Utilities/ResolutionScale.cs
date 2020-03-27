using UnityEngine;
using UnityEngine.XR;

public class ResolutionScale : MonoBehaviour
{
    public float resolutionScale = 1;
    private void Awake()
    {
        XRSettings.eyeTextureResolutionScale = resolutionScale;
    }
}
