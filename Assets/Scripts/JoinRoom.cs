using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinRoom : MonoBehaviour
{
    public string roomName;

    public void ChangeRoom(string _roomName)
    {
        roomName = _roomName;
    }
}
