using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class InGameRoomDisplay : MonoBehaviour
{
    public TextMeshProUGUI roomName;
    // Start is called before the first frame update
    void Start()
    {
        roomName.text = $"Room Name: {PhotonNetwork.CurrentRoom.Name}";
    }
}
