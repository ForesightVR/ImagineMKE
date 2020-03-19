using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class NetworkConnectionManager : MonoBehaviourPunCallbacks
{
    public static NetworkConnectionManager Instance;
    public string roomName;

    private string adminPassword = "foresightAdmin";
    public bool IsAdmin { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        roomName = RoomSelection.Instance.rooms[0].roomName;
    }

    public void ChangeRoomName(Room room)
    {
        roomName = room.roomName;
    }

    public void SetNickName(TMP_InputField inputField)
    {
        PhotonNetwork.NickName = inputField.text;
    }

    public void CheckAdminStatus(TMP_InputField inputField)
    {
        if (inputField.text == adminPassword)
            IsAdmin = true;
        else
            IsAdmin = false;
    }

    public void ConnectToMaster()
    {
        PhotonNetwork.OfflineMode = false;
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

        Debug.Log("Connecting To Room");
        PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions { MaxPlayers = 30 }, new TypedLobby("Main Lobby", LobbyType.Default));
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to Master!");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log("Disconnected");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Room Joined!");
        SceneManager.LoadScene(roomName);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("No Room Available! Creating new room...");

        //PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 10 });
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.LogError(message);
    }
}