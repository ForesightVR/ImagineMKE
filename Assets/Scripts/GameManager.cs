﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public Player playerPrefab;
    public string menuSceneName;

    Player localPlayer;

    private void Awake()
    {
        if(!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene(menuSceneName);
            return;
        }
    }

    private void Start()
    {
        //Player.RefereshInstance(ref localPlayer, playerPrefab);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        if(Player.LocalPlayerInstance == null)
            PhotonNetwork.Instantiate(playerPrefab.gameObject.name, Vector3.zero, Quaternion.identity);
        //Player.RefereshInstance(ref localPlayer, playerPrefab);
    }
}
