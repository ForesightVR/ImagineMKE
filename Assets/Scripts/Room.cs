using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Room : MonoBehaviour
{
    public RoomSelection roomSelection;
    public TextMeshProUGUI roomCounter;
    public string roomName;
    [TextArea]
    public string roomDescription;

    public void UpdateRoomInfo()
    {
        Debug.Log("Update Room Info");
        roomSelection.UpdateRoomInfo(roomName, roomDescription);
    }
}
