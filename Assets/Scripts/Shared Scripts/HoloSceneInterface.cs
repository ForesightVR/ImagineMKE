using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Valve.VR;

public class HoloSceneInterface : MonoBehaviour
{
    public Image logoImage;
    public TextMeshProUGUI descriptionText;
    public SteamVR_LoadLevel sceneLoader;

    string scene;

    public void SetInfo(Sprite logo, string description, string sceneName)
    {
        logoImage.sprite = logo;
        descriptionText.text = description;
        scene = sceneName;
    }

    public void LoadScene()
    {
        if (sceneLoader == null)
            sceneLoader = GameObject.FindObjectOfType<SteamVR_LoadLevel>();

        if (scene != "")
        {
            sceneLoader.levelName = scene;
            sceneLoader.Trigger();
            transform.root.position = Vector3.zero;
        }
    }
}
