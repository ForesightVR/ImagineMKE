using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kick : MonoBehaviour
{
    PlayerList playerList;

    private void Start()
    {
        playerList = GetComponentInParent<PlayerList>();
    }

    public void KickPlayer(PlayerInfo playerInfo)
    {
        playerList.KickPlayer(playerInfo);
    }
}
