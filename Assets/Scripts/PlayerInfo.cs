using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Michsky.UI.ModernUIPack;
using Dissonance.Audio.Playback;

public class PlayerInfo : MonoBehaviour
{
    public TextMeshProUGUI playerName;
    public GameObject kickButton;
    public SliderManager volumeSlider;
    AudioSource source;

    public Photon.Realtime.Player Player { get; private set; }

    public void SetInfo(Photon.Realtime.Player player)
    {
        playerName.text = player.NickName;
        Player = player;

        GameObject dissonance = GameObject.FindGameObjectWithTag("Dissonance");

        var voiceChats = dissonance.transform.GetComponentsInChildren<VoicePlayback>();

        if (voiceChats.Length > 0)
        {
            var voice = voiceChats.FirstOrDefault(x => x.PlayerName == player.UserId);

            if (voice != null)
            {
                source = voice.AudioSource;
                volumeSlider.mainSlider.onValueChanged.AddListener(delegate { ChangeVolume(volumeSlider.mainSlider.value); });
            }
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
        }
    }
}
