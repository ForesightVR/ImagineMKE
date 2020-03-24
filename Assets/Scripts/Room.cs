using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Room : MonoBehaviour
{
    public RoomSelection roomSelection;
    public TextMeshProUGUI roomCounter;
    public string roomName;
    [TextArea]
    public string roomDescription;

    public void UpdateRoomInfo()
    {
        roomSelection.UpdateRoomInfo(roomName, roomDescription);
    }

    public void UpdateRoomCounter(int currentCount, int maxCount)
    {
        roomCounter.text = $"{currentCount} / {maxCount}";
    }
}
