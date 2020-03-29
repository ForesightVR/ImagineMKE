using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Foresight;
using Player = Foresight.Player;
using Dissonance;

public class GameManager : MonoBehaviourPunCallbacks
{
    public Player playerPrefab;
    public string menuSceneName;

    Player localPlayer;
    DissonanceComms Comms;

    private void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            ReturnToMenu();
            return;
        }

        Comms = Comms ?? FindObjectOfType<DissonanceComms>();
        Comms.Rooms.Join("Global");
    }

    private void Start()
    {
        if (Player.LocalPlayerInstance == null)
        {
            PhotonNetwork.Instantiate(playerPrefab.gameObject.name, Vector3.zero, Quaternion.identity);
            PhotonNetwork.LocalPlayer.SetAdminStatus(NetworkConnectionManager.Instance.IsAdmin);
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {

        base.OnPlayerEnteredRoom(newPlayer);
        if(Player.LocalPlayerInstance == null)
            PhotonNetwork.Instantiate(playerPrefab.gameObject.name, Vector3.zero, Quaternion.identity);

        //Player.RefereshInstance(ref localPlayer, playerPrefab);
    }

    public void LeaveRom()
    {
        PhotonNetwork.LeaveRoom();
        ReturnToMenu();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
