using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Michsky.UI.ModernUIPack;
using Dissonance.Audio.Playback;

public class PlayerInfo : MonoBehaviour
{
    public TextMeshProUGUI playerName;
    public GameObject kickButton;
    public ButtonManagerBasic muteButton;
    public SliderManager volumeSlider;
    AudioSource source;

    //Mute Variables
    float lastVolume;
    bool isMute;

    public Photon.Realtime.Player Player { get; private set; }

    public void SetInfo(Photon.Realtime.Player player)
    {
        playerName.text = player.NickName;
        Player = player;

        GameObject dissonance = GameObject.FindGameObjectWithTag("Dissonance");

        var voiceChats = dissonance.transform.GetComponentsInChildren<VoicePlayback>();

        var voice = voiceChats.FirstOrDefault(x => x.PlayerName == player.UserId);

        if (voice != null)
        {
            source = voice.AudioSource;
            volumeSlider.mainSlider.onValueChanged.AddListener(delegate { ChangeVolume(volumeSlider.mainSlider.value); });
            lastVolume = volumeSlider.mainSlider.value;
        }
        else //there is no voicePlayback, so we shouldn't have the UI do anything
        {
            volumeSlider.gameObject.SetActive(false);
            if(muteButton)
                muteButton.gameObject.SetActive(false);
        }
        
    }

    public void SetAdmin()
    {
        kickButton.SetActive(true);
    }

    public void ChangeVolume(float changedValue)
    {
        if (source != null)
        {
            source.volume = changedValue;

            if (changedValue != 0)
                isMute = false;
        }
    }

    public void MuteVolume()
    {
        isMute = !isMute;

        if (isMute)
        {
            lastVolume = source.volume;
            volumeSlider.mainSlider.value = 0;
            source.volume = 0;
            muteButton.buttonText = "Unmute";
        }
        else
        {
            source.volume = lastVolume;
            volumeSlider.mainSlider.value = lastVolume;
            muteButton.buttonText = "Mute";
        }
    }
}
