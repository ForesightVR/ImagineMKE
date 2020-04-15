using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectToRoom : MonoBehaviour
{
    public bool isJoinRoomBtn;
    public GameObject loadingScreen;
    public void ConnectPreset()
    {
        if (NetworkConnectionManager.Instance)
        { 
            NetworkConnectionManager.Instance.ConnectToRoom();
            loadingScreen.SetActive(true);
        }
            
    }

    public void ConnectRandom()
    {
        if (NetworkConnectionManager.Instance)
        {
            NetworkConnectionManager.Instance.ConnectToRandomRoom();
            loadingScreen.SetActive(true);
        }
            
    }

    private void Update()
    {
        if (isJoinRoomBtn)
            gameObject.SetActive(!string.IsNullOrWhiteSpace(NetworkConnectionManager.Instance.roomName));
    }
}
