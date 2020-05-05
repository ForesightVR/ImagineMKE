using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Photon.Pun;

public class CheckName : MonoBehaviour
{
    public GameObject nameWarning;
    public UnityEvent unityEvent;

    public void Enter(TMP_InputField inputField)
    {
        if (string.IsNullOrWhiteSpace(inputField.text))
            nameWarning.SetActive(true);
        else
        {
            if (!PhotonNetwork.IsConnected)
                NetworkConnectionManager.Instance.ConnectToMaster();
            else
                unityEvent?.Invoke();
        }
    }
}
