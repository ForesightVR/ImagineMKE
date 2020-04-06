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
    public static GameManager Instance;

    public Player playerPrefab;
    public string menuSceneName;

    public GameObject[] menus;
    public bool MenuOpen { get; private set; }

    RoomMembership roomMembership;
    DissonanceComms Comms;

//Test
    private void Awake()
    {
        Instance = this;
        if (!PhotonNetwork.IsConnected)
        {
            ReturnToMenu();
            return;
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Comms = Comms != null ? Comms : FindObjectOfType<DissonanceComms>();
        roomMembership = Comms.Rooms.Join(NetworkConnectionManager.Instance.roomName);
        Debug.Log(roomMembership.RoomName);
    }

    private void Start()
    {
        if (Player.LocalPlayerInstance == null)
        {
            PhotonNetwork.Instantiate(playerPrefab.gameObject.name, Vector3.zero, Quaternion.identity);
            PhotonNetwork.LocalPlayer.SetAdminStatus(NetworkConnectionManager.Instance.IsAdmin);
        }
    }

    private void Update()
    {
        foreach(GameObject go in menus)
        {
            if (go.activeInHierarchy)
            {
                MenuState(true);
                return;
            }
        }

        MenuState(false);
    }

    public void MenuState(bool state)
    {
        MenuOpen = state;
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        if (Player.LocalPlayerInstance == null)
        {
            PhotonNetwork.Instantiate(playerPrefab.gameObject.name, Vector3.zero, Quaternion.identity);
        }            
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();        
        Comms.Rooms.Leave(roomMembership);
        ReturnToMenu();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
