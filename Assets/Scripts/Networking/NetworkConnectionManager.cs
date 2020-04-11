﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class NetworkConnectionManager : MonoBehaviourPunCallbacks
{
    public static NetworkConnectionManager Instance;
    public string sceneName = "Outsider's Gallery";
    public string roomName;
    public byte maxPlayers = 15;
    public byte CharacterSelected { get; private set; }

    public TypedLobby typedLobby = new TypedLobby("mainLobby", LobbyType.SqlLobby);

    private string adminPassword = "foresightAdmin";
    public bool IsAdmin { get; private set; }
    public PhotonDissonanceBridge bridge;

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

    public void SetCharacter(byte characterIndex)
    {
        CharacterSelected = characterIndex;
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

        PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions { MaxPlayers = maxPlayers , PublishUserId = true}, typedLobby);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby(typedLobby);
        bridge.SetPlayerId();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
    }

    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        RoomSelection.Instance.rooms.ForEach(x =>
        {
            if (roomList.Count > 0)
            {
                var room = roomList.FirstOrDefault(y => y.Name == x.roomName);

                if (room != null && room.MaxPlayers > 0)
                {
                    x.UpdateRoomCounter(room.PlayerCount, room.MaxPlayers);
                    return;
                }                    
            }

            x.UpdateRoomCounter(0, maxPlayers);
        });
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        SceneManager.LoadScene(sceneName);
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
    }
}