using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using Photon.Realtime;

public class PlayerList : MonoBehaviourPunCallbacks
{
    public PlayerInfo playerInfoPrefab;
    public GameObject playerList;
    public GameObject playerInfoHolder;

    public List<PlayerInfo> playerInfos = new List<PlayerInfo>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            TogglePlayerList(!playerList.activeSelf);
    }

    public void TogglePlayerList(bool activeState)
    {
        if(activeState == true)
            UpdatePlayerList();

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

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        PlayerInfo playerInfo = playerInfos.FirstOrDefault(x => x.playerName.text == otherPlayer.NickName);

        if (playerInfo)
            playerInfos.Remove(playerInfo);
    }
}
