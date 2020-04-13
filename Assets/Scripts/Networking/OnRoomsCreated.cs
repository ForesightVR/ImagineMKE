using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class OnRoomsCreated : MonoBehaviourPunCallbacks
{
    public UnityEvent unityEvent;

    int numberOfRooms;

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        numberOfRooms++;
        if (numberOfRooms >= RoomSelection.Instance.roomUIs.Count)
        {
            unityEvent?.Invoke();
            numberOfRooms = 0;
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.LogError(message);
    }
}
