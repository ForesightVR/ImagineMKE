using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Room : MonoBehaviour
{
    public RoomSelection roomSelection;
    public TextMeshProUGUI roomCounter; 
    public TextMeshProUGUI roomNameDisplay;
    public string roomName;
    [TextArea]
    public string roomDescription;

    public void UpdateRoomInfo()
    {
        roomSelection.UpdateRoomInfo(roomName, roomDescription);
    }

    public void UpdateRoomCounter(string roomName, int currentCount, int maxCount)
    {
        roomNameDisplay.text = $"{roomName}";
        roomCounter.text = $"{currentCount} / {maxCount}";
    }
}
