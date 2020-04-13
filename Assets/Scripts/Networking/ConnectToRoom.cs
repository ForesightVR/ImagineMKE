using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectToRoom : MonoBehaviour
{    
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
}
