using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Michsky.UI.ModernUIPack;

public class SoundManager : MonoBehaviour
{
    public AudioMixer mixer;
    public GameObject volumeCanvas;
    public bool ignoreInput;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !ignoreInput)
            volumeCanvas.SetActive(!volumeCanvas.activeInHierarchy);
    }

    public void ChangeMasterVolume(SliderManager sliderManager)
    {
        ChangeVolume("MasterVolume", sliderManager.mainSlider.value);
    }

    public void ChangeMusicVolume(SliderManager sliderManager)
    {
        ChangeVolume("MusicVolume", sliderManager.mainSlider.value);
    }

    public void ChangeEnvironmentVolume(SliderManager sliderManager)
    {
        ChangeVolume("EnvironmentVolume", sliderManager.mainSlider.value);
    }

    public void ChangeVoiceChatVolume(SliderManager sliderManager)
    {
        ChangeVolume("VoiceChatVolume", sliderManager.mainSlider.value);
    }

    void ChangeVolume(string group, float volume)
    {
        mixer.SetFloat(group, volume);
    }
}
