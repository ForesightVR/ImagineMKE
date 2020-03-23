using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectToRoom : MonoBehaviour
{
    public void Connect()
    {
        if (NetworkConnectionManager.Instance)
            NetworkConnectionManager.Instance.ConnectToRoom();
    }
}
