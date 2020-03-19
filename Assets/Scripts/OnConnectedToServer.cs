using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class OnConnectedToServer : MonoBehaviourPunCallbacks
{
    public UnityEvent unityEvent;

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("E _ ONConnected");
        unityEvent?.Invoke();
    }

}
