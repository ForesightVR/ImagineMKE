using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkConnectionManager : MonoBehaviourPunCallbacks
{
    public string mainSceneName;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void ConnectToMaster()
    {
        PhotonNetwork.OfflineMode = false;
        PhotonNetwork.NickName = "PlayerName";
        PhotonNetwork.GameVersion = "v1";

        PhotonNetwork.ConnectUsingSettings();
    }

    public void ConnectToRoom()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogError("Not Connected To Photon Network!");
            return;
        }

        //Other Options for joining a room.
        //PhotonNetwork.CreateRoom("Room Name") //Create a specific room -- Error Callback: OnCreateRoomFailed()
        //PhotonNetwor.JoinRoom("Room Name") //Join a specific room -- Error Callback: OnJoinRoomFailed()

        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to Master!");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.LogError(cause);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Room Joined!");
        SceneManager.LoadScene(mainSceneName);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("No Room Available! Creating new room...");

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 10 });
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.LogError(message);
    }
}