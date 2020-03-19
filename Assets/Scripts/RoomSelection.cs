using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomSelection : MonoBehaviour
{
    public static RoomSelection Instance;
    public List<Room> rooms;
    public TextMeshProUGUI roomName;
    public TextMeshProUGUI roomDescription;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateRoomInfo(string _roomName, string _roomDescription)
    {
        roomName.text = _roomName;
        roomDescription.text = _roomDescription;
    }
}
