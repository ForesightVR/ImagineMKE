using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PlayerList : MonoBehaviourPunCallbacks
{
    public PlayerInfo playerInfoPrefab;
    public GameObject playerList;
    public GameObject playerInfoHolder;

    public List<PlayerInfo> playerInfos = new List<PlayerInfo>();

    public GameObject chatInput;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePlayerList(!playerList.activeSelf);

        GameManager.Instance.MenuState(playerList.activeSelf || chatInput.activeSelf);
    }

    public void TogglePlayerList(bool activeState)
    {
        if(activeState == true)
        {
            UpdatePlayerList();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        playerList.SetActive(activeState);
    }

    void UpdatePlayerList()
    {
        foreach(Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            PlayerInfo playerInfo = playerInfos.FirstOrDefault(x => x.playerName.text == player.NickName);

            if (playerInfo == null)
            {
                PlayerInfo newPlayerInfo = Instantiate(playerInfoPrefab, playerInfoHolder.transform);
                newPlayerInfo.SetInfo(player);
                playerInfos.Add(newPlayerInfo);
            }
        }

        if(PhotonNetwork.LocalPlayer.IsAdmin)
        {
            foreach (PlayerInfo info in playerInfos)
                info.SetAdmin();
        }
    }

    public void KickPlayer(PlayerInfo playerToKick)
    {
        if (PhotonNetwork.LocalPlayer.IsAdmin)
        {
            PhotonNetwork.CloseConnection(playerToKick.Player);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        //Debug.Log("Disconnected");
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        //Debug.Log("OnPlayerLeftRoom");
        base.OnPlayerLeftRoom(otherPlayer);
        PlayerInfo playerInfo = playerInfos.FirstOrDefault(x => x.playerName.text == otherPlayer.NickName);

        if (playerInfo)
        {
            playerInfos.Remove(playerInfo);
            Destroy(playerInfo.gameObject);
        }
    }
}
