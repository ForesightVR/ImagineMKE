using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Michsky.UI.ModernUIPack;

public class PlayerInfo : MonoBehaviour
{
    public TextMeshProUGUI playerName;
    public GameObject kickButton;
    public SliderManager volumeSlider;

    public Photon.Realtime.Player Player { get; private set; }

    public void SetInfo(Photon.Realtime.Player player)
    {
        playerName.text = player.NickName;
        Player = player;
    }

    public void SetAdmin()
    {
        kickButton.SetActive(true);
    }

    public void ChangeVolume()
    {
        //Chance the volume of voice chat to volumeSlider.currentValue;
    }
}
