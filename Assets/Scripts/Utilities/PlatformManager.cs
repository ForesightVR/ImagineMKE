using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlatformManager : MonoBehaviour
{
    public void EnterInVR()
    {
        XRSettings.enabled = true;
        gameObject.SetActive(false);
    }
    
    public void EnterInPC()
    {
        XRSettings.enabled = false;
        gameObject.SetActive(false);
    }
}
