using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkErrors : MonoBehaviour
{
    public GameObject error;
    public GameObject joinButton;
    // Update is called once per frame
    void Update()
    {
        error.SetActive(!PhotonNetwork.IsConnected);
        joinButton.SetActive(PhotonNetwork.IsConnected);
    }
}
