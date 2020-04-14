using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectToRoom : MonoBehaviour
{
    public bool isJoinRoomBtn;
    public void ConnectPreset()
    {
        if (NetworkConnectionManager.Instance)
            NetworkConnectionManager.Instance.ConnectToRoom();
    }

    public void ConnectRandom()
    {
        if (NetworkConnectionManager.Instance)
            NetworkConnectionManager.Instance.ConnectToRandomRoom();
    }

    private void Update()
    {
        if (isJoinRoomBtn)
            gameObject.SetActive(!string.IsNullOrWhiteSpace(NetworkConnectionManager.Instance.roomName));
    }
}
