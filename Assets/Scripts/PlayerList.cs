using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class PlayerList : MonoBehaviour
{
    public PlayerInfo playerInfoPrefab;
    public GameObject playerList;
    public GameObject playerInfoHolder;

    public List<PlayerInfo> playerInfos = new List<PlayerInfo>();

    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
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
    }
}
