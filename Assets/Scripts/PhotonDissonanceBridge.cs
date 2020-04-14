using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Dissonance;

public class PhotonDissonanceBridge
    : MonoBehaviour
{
    public static GameObject Instance;
    DissonanceComms dissonance;

    private void Start()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
            Instance = gameObject;
        else
            Destroy(gameObject);

        dissonance = GetComponent<DissonanceComms>();
    }

    public void SetPlayerId()
    {
        Debug.Log("RoomName: " + NetworkConnectionManager.Instance.roomName);
        dissonance.LocalPlayerName = PhotonNetwork.LocalPlayer.UserId;
        dissonance.roomName = NetworkConnectionManager.Instance.roomName;
        dissonance.enabled = true;
    }
}
